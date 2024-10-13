#!/bin/bash

# Exit script on any error
set -e

# Set ENV
export ENV_FILE=.env
source $ENV_FILE

# Set up environment variables for Docker Compose
echo "Setting up environment variables for Docker Compose..."
echo "ENV_NAME=$ENV_NAME" >> $ENV_FILE
echo "SQL_DB_CONTAINER=$SQL_DB_CONTAINER" >> $ENV_FILE
echo "SQL_DB_NAME=$SQL_DB_NAME" >> $ENV_FILE
echo "SQL_DB_PASSWORD=$SQL_DB_PASSWORD" >> $ENV_FILE
echo "API_CONTAINER=$API_CONTAINER" >> $ENV_FILE
echo "WEB_CONTAINER=$WEB_CONTAINER" >> $ENV_FILE
echo "DOCKER_REGISTRY=$DOCKER_ALPHAVISA_REPO" >> $ENV_FILE
echo "APP_CURRENT_TAG=$WEB_TAG" >> $ENV_FILE
echo "SERVER_API_URL=$SERVER_API_URL" >> $ENV_FILE
echo "SQL_DATA_PATH=$SQL_DATA_PATH" >> $ENV_FILE
echo "FILE_STORE_PATH=$FILE_STORE_PATH" >> $ENV_FILE

# Re-source the .env file to load the newly appended variables
source $ENV_FILE

# Update system and install prerequisites
echo "Updating system and installing prerequisites..."
sudo apt-get update && sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common jq unzip zip

# Install Docker
echo "Installing Docker..."
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
sudo apt-get update
sudo apt-get install -y docker-ce

# Add current user to Docker group
echo "Adding current user to Docker group..."
sudo usermod -aG docker $USER
sudo chmod 666 /var/run/docker.sock
# Optional: newgrp docker //Reload shell to apply user group changes

# Install Docker Compose (latest version)
echo "Installing Docker Compose..."
# TODO: sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Run docker compose file


# Install Nginx
echo "Installing Nginx..."
sudo apt-get update
sudo apt-get install -y nginx

# Setup Nginx for $API_DOMAIN (port 80 only)
echo "Setting up Nginx for $API_DOMAIN on port 80..."
cat <<EOF | sudo tee /etc/nginx/sites-available/$API_DOMAIN
server {
    listen 80;
    server_name $API_DOMAIN;

    location /.well-known/acme-challenge/ {
        root /var/www/html;
        allow all;
    }
	
    location / {
        proxy_pass http://localhost:$API_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
}
EOF

# Enable the Nginx configuration for port 80
sudo ln -sf /etc/nginx/sites-available/$API_DOMAIN /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx

# Install lego for Let's Encrypt SSL certificates
echo "Installing Lego..."
sudo curl -L "https://github.com/go-acme/lego/releases/download/v4.5.3/lego_v4.5.3_linux_amd64.tar.gz" -o lego.tar.gz
sudo tar -xf lego.tar.gz -C /usr/local/bin lego
sudo chmod +x /usr/local/bin/lego

# Obtain SSL certificates
echo "Obtaining SSL certificates..."
sudo lego --email=$APP_MAIL --domains=$API_DOMAIN --http --http.webroot /var/www/html --path="/etc/lego" run

# Once SSL certificates are obtained, update Nginx configuration to enable HTTPS (port 443)
echo "Updating Nginx to use SSL on port 443..."
cat <<EOF | sudo tee -a /etc/nginx/sites-available/$API_DOMAIN

server {
    listen 443 ssl;
    server_name $API_DOMAIN;

    ssl_certificate /etc/lego/certificates/$API_DOMAIN.crt;
    ssl_certificate_key /etc/lego/certificates/$API_DOMAIN.key;

    location / {
        proxy_pass http://localhost:$API_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
	# Global setting: client_max_body_size 5M;
	# Optional: Define a specific location for AttachmentItems
    location /api/v1/AttachmentItems {
        client_max_body_size 5M;
        proxy_pass http://localhost:$API_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
}
EOF

# Reload Nginx to apply SSL settings
sudo nginx -t
sudo systemctl reload nginx

# Setup Nginx for $WEB_DOMAIN (port 80 only)
echo "Setting up Nginx for $WEB_DOMAIN on port 80..."
cat <<EOF | sudo tee /etc/nginx/sites-available/$WEB_DOMAIN
server {
    listen 80;
    server_name $WEB_DOMAIN;

    location /.well-known/acme-challenge/ {
        root /var/www/html;
        allow all;
    }
	
    location / {
        proxy_pass http://localhost:$WEB_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
}
EOF

# Enable the Nginx configuration for port 80
sudo ln -sf /etc/nginx/sites-available/$WEB_DOMAIN /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx

# Obtain SSL certificates
echo "Obtaining SSL certificates..."
sudo lego --email=$APP_MAIL --domains=$WEB_DOMAIN --http --http.webroot /var/www/html --path="/etc/lego" run

# Once SSL certificates are obtained, update Nginx configuration to enable HTTPS (port 443)
echo "Updating Nginx to use SSL on port 443..."
cat <<EOF | sudo tee -a /etc/nginx/sites-available/$WEB_DOMAIN

server {
    listen 443 ssl;
    server_name $WEB_DOMAIN;

    ssl_certificate /etc/lego/certificates/$WEB_DOMAIN.crt;
    ssl_certificate_key /etc/lego/certificates/$WEB_DOMAIN.key;

    location / {
        proxy_pass http://localhost:$WEB_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
}
EOF

# Reload Nginx to apply SSL settings
sudo nginx -t
sudo systemctl reload nginx

# Create neccessary folders for docker compose
sudo mkdir -p $SQL_DATA_PATH
sudo chown 10001:10001 $SQL_DATA_PATH
sudo chmod 700 $SQL_DATA_PATH
sudo mkdir -p $FILE_STORE_PATH

# Starting docker compose
echo "Starting Docker Compose services..."
docker-compose --env-file $ENV_FILE down
docker-compose --env-file $ENV_FILE up -d

# Install .NET SDK and Runtimes (8.x)
echo "Installing .NET SDK and Runtimes..."

# Detect OS version
source /etc/os-release

if [[ $VERSION_ID == "20.04" ]]; then
  echo "Detected Ubuntu 20.04. Installing .NET SDK and Runtimes for Ubuntu 20.04..."
  
  # Download and install Microsoft packages
  wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  rm packages-microsoft-prod.deb

  sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
  sudo apt-get install -y aspnetcore-runtime-8.0

elif [[ $VERSION_ID == "24.04" ]]; then
  echo "Detected Ubuntu 24.04. Installing .NET SDK and Runtimes for Ubuntu 24.04..."
  
  sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
  sudo apt-get install -y aspnetcore-runtime-8.0

else
  echo "Unsupported Ubuntu version: $VERSION_ID. Exiting..."
  exit 1
fi

# Re-source for env config
source $ENV_FILE

# Set up .NET SDK path if needed
DOTNET_INSTALL_DIR="/usr/share/dotnet"
export PATH=$DOTNET_INSTALL_DIR:$PATH
export DOTNET_ROOT=$DOTNET_INSTALL_DIR

# Add to PATH for future sessions
if ! grep -q "$DOTNET_INSTALL_DIR" /etc/profile; then
  echo "export PATH=\$PATH:$DOTNET_INSTALL_DIR" | sudo tee -a /etc/profile
  echo "export DOTNET_ROOT=$DOTNET_INSTALL_DIR" | sudo tee -a /etc/profile
fi

# Reload shell to apply new PATH
source /etc/profile

# Verify the installation
dotnet --version

echo "Installation of all required tools is complete!"

echo "Starting Docker Compose services..."
docker-compose --env-file $ENV_FILE down
docker-compose --env-file $ENV_FILE up -d

# Wait for SQL Server to be available
echo "Waiting for SQL Server to be available..."
timeout 120s bash -c 'until docker exec $SQL_DB_CONTAINER /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SQL_DB_PASSWORD -No -Q "SELECT 1" > /dev/null 2>&1; do
    echo "SQL Server is not available yet, waiting..."
    sleep 5
done'
if [ $? -ne 0 ]; then
    echo "Timeout reached, SQL Server did not become ready in time."
    exit 1
fi
echo "SQL Server is ready."

# Apply EF Core migrations
echo "Applying EF Core migrations..."
unzip -o efbundle.zip
./efbundle.exe --connection "Server=$SQL_DB_SERVER;Database=$SQL_DB_NAME;User ID=sa;Password=$SQL_DB_PASSWORD;Encrypt=true;TrustServerCertificate=true;" || true

# Wait for API to be available
echo "Waiting for API to be available..."
timeout 120s bash -c 'until curl -sSf http://localhost:$API_PORT > /dev/null 2>&1; do
    echo "API is not available yet, waiting..."
    sleep 5
done'
if [ $? -ne 0 ]; then
    echo "Timeout reached, API did not become ready in time."
    exit 1
fi
echo "API is ready."

# Post actions
sudo systemctl reload nginx
curl -f http://localhost:8080 || exit 1
echo "Setup complete and services are running successfully!"

####################################### Github runner registration
# Create a folder
mkdir ~/actions-runner && cd ~/actions-runner
# Download the latest runner package
curl -o actions-runner-linux-x64-2.320.0.tar.gz -L https://github.com/actions/runner/releases/download/v2.320.0/actions-runner-linux-x64-2.320.0.tar.gz
# Optional: Validate the hash
echo "93ac1b7ce743ee85b5d386f5c1787385ef07b3d7c728ff66ce0d3813d5f46900  actions-runner-linux-x64-2.320.0.tar.gz" | shasum -a 256 -c
# Extract the installer
tar xzf ./actions-runner-linux-x64-2.320.0.tar.gz
# Config
echo "" | echo "" | echo "" | ./config.sh --url $RUNNER_REPO --token $RUNNER_TOKEN
sudo ./svc.sh install
sudo ./svc.sh start
