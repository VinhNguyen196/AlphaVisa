#!/bin/bash

# Set ENV from .env
source .env
ENV_CONFIG=$APP_PATH/$ENV_FILE
source $ENV_CONFIG
cd $APP_PATH

# Define the backup directory
BACKUP_DIR=$APP_BACKUP_PATH
BACKUP_FILE=$BACKUP_DIR/deployment_backup_$(date +"%Y%m%d%H%M%S").tar.gz

# Create the backup directory if it doesn't exist
mkdir -p $BACKUP_DIR

# Use mktemp to create a secure temporary directory for intermediate files
TEMP_BACKUP_DIR=$(mktemp -d)

# Backup SQL Database
echo "Backing up SQL Database..."
docker exec $SQL_DB_CONTAINER /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SQL_DB_PASSWORD -No -Q "BACKUP DATABASE [$SQL_DB_NAME] TO DISK = N'/var/opt/mssql/backup/$SQL_DB_NAME.bak'"

# Copy the backup file from SQL_DATA_PATH to TEMP_BACKUP_DIR
echo "Copying SQL backup file to local backup directory..."
docker cp $SQL_DB_CONTAINER:/var/opt/mssql/backup/$SQL_DB_NAME.bak $TEMP_BACKUP_DIR/

# Backup filestore
echo "Backing up filestore..."
mkdir -p $TEMP_BACKUP_DIR/filestore
cp -r $FILE_STORE_PATH/* $TEMP_BACKUP_DIR/filestore/

# Backup efbundle
echo "Backing up efbundle..."
cp efbundle.zip $TEMP_BACKUP_DIR/

# Backup .env file
echo "Backing up .env file..."
cp $ENV_CONFIG $TEMP_BACKUP_DIR/
echo "BACKUP_FILE=$BACKUP_FILE" >> $TEMP_BACKUP_DIR/$ENV_FILE

# Backup docker compose file
echo "Backing up docker-compose file..."
cp docker-compose.yml $TEMP_BACKUP_DIR/

# Compress all backup files into a tarball
echo "Compressing backup files into a tarball..."
tar -czf $BACKUP_FILE -C $TEMP_BACKUP_DIR .

# Cleanup intermediate files
echo "Cleaning up intermediate files..."
rm -rf $TEMP_BACKUP_DIR

echo "Backup completed. Compressed file created: $BACKUP_FILE"
