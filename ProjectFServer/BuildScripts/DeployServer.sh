#!/bin/bash

# === 설정값 ===
SSH_CONFIG_PATH=./.ssh/config
SERVER_NAME=ProjectFServer
REMOTE_USER=seh00n
REMOTE_PATH=/home/$REMOTE_USER/ProjectFServer
SERVICE_NAME=ProjectFServer
PROJECT_PATH=./ProjectFServer.csproj     # 프로젝트 파일 경로
PUBLISH_DIR=./publish  # publish task가 여기에 생성한다고 가정

# === 0. Prepare Build Folder ===
echo "[DeployServer::PrepareBuildFolder] clean build folder..."
rm -rf $PUBLISH_DIR

# === 1. Publish ===
echo "[DeployServer::PublishProject] publishing project..."
dotnet publish "$PROJECT_PATH" -c Release -o "$PUBLISH_DIR"

if [ $? -ne 0 ]; then
    echo "publish failed."
    exit 1
fi

# === 2. 파일 복사 ===
echo "[DeployServer::CopyFile] copying files to remote server..."
rsync -avz -e "ssh -F $SSH_CONFIG_PATH" "$PUBLISH_DIR"/ "$SERVER_NAME:$REMOTE_PATH"

if [ $? -ne 0 ]; then
    echo "[DeployServer::CopyFile] rsync failed."
    exit 1
fi

# === 3. 서비스 재시작 ===
echo "[DeployServer::RestartService] Restarting service on remote server..."
ssh -F $SSH_CONFIG_PATH $SERVER_NAME "sudo systemctl restart $SERVICE_NAME"

if [ $? -eq 0 ]; then
    echo "[DeployServer::RestartService] Deployment complete! Service restarted successfully."
else
    echo "[DeployServer::RestartService] service restart failed."
fi

# === 3^. 서비스 재시작 대기 ===
echo "[DeployServer::WaitForRestartService] Waiting Server Uptime"
sleep 10

# === 4. 로그 보기 ===
echo "[DeployServer::ShowLog] Showing latest service logs (Ctrl+C to exit)"
ssh -F $SSH_CONFIG_PATH $SERVER_NAME "sudo journalctl -u $SERVICE_NAME -n 30"