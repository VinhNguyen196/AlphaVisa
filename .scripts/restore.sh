#!/bin/bash

# Set ENV from .env
source .env
ENV_CONFIG=$APP_PATH/$ENV_FILE
source $ENV_CONFIG
cd $APP_PATH

# Find the first .tar.gz backup file in APP_BACKUP_PATH
BACKUP_FILE=$(find $APP_BACKUP_PATH -maxdepth 1 -name "*.tar.gz" | head -n 1)

# Check if a backup file was found
if [ -z "$BACKUP_FILE" ]; then
  echo "No backup file found in $APP_BACKUP_PATH."
  exit 1
fi

# Inform user of the found backup file
echo "Found backup file: $BACKUP_FILE"

# Create a temporary directory for restoration using mktemp
RESTORE_DIR=$(mktemp -d)

# Extract the tarball to the temporary directory
echo "Extracting backup files from $BACKUP_FILE..."
tar -xzf $BACKUP_FILE -C $RESTORE_DIR

# Source the deployment ENV
source $RESTORE_DIR/$ENV_FILE

# Restore SQL Database later on run.sh when docker containers start
echo "Restoring SQL Database... later on run.sh when docker containers start"

# Restore filestore
echo "Restoring filestore..."
cp -r $RESTORE_DIR/filestore/* $FILE_STORE_PATH/

# Restore efbundle
echo "Restoring efbundle..."
cp $RESTORE_DIR/efbundle.zip $APP_PATH/

# Restore .env file
echo "Restoring .env file..."
cp $RESTORE_DIR/$ENV_FILE $ENV_CONFIG

# Backup docker compose file
echo "Backing up docker-compose file..."
cp $RESTORE_DIR/docker-compose.yml $APP_PATH/

# Clean up temporary restore directory later in run.sh
echo "RESTORE_DIR=$RESTORE_DIR" >> $ENV_CONFIG
echo "Restore completed successfully at $RESTORE_DIR"
