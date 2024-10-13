#!/bin/bash

# Set ENV from .env
source .env
ENV_CONFIG=$ENV_PATH/$ENV_FILE
source $ENV_CONFIG
cd $APP_PATH

# Check if the backup file exists
if [ ! -f $BACKUP_FILE ]; then
  echo "Backup file $BACKUP_FILE does not exist."
  exit 1
fi

# Define temporary restore directory
RESTORE_DIR=./restore_tmp
mkdir -p $RESTORE_DIR

# Extract the tarball
echo "Extracting backup files from $BACKUP_FILE..."
tar -xzf $BACKUP_FILE -C $RESTORE_DIR

# Restore SQL Database
echo "Restoring SQL Database..."
docker cp $RESTORE_DIR/$SQL_DB_NAME.bak $SQL_DB_CONTAINER:/var/opt/mssql/backup/
docker exec $SQL_DB_CONTAINER /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SQL_DB_PASSWORD -No -Q "RESTORE DATABASE [$SQL_DB_NAME] FROM DISK = N'/var/opt/mssql/backup/$SQL_DB_NAME.bak' WITH REPLACE"

# Restore filestore
echo "Restoring filestore..."
cp -r $RESTORE_DIR/filestore/* $FILE_STORE_PATH/

# Restore efbundle
echo "Restoring efbundle..."
cp $RESTORE_DIR/efbundle.zip ./efbundle.zip

# Restore .env file
echo "Restoring .env file..."
cp $RESTORE_DIR/$ENV_FILE $ENV_CONFIG

# Clean up temporary restore directory
rm -rf $RESTORE_DIR

echo "RESTORE_DIR=$RESTORE_DIR" >> $ENV_CONFIG
echo "Restore completed successfully at $RESTORE_DIR"