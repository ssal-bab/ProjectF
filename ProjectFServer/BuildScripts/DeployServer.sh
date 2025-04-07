#!/bin/bash

# === 설정값 ===
REMOTE_USER=seh00n
REMOTE_HOST=seh00n.iptime.org
REMOTE_PORT=10802
REMOTE_PATH=/home/$REMOTE_USER/ProjectFServer
SERVICE_NAME=ProjectFServer
PROJECT_PATH=./ProjectFServer.csproj     # 프로젝트 파일 경로
PUBLISH_DIR=./publish  # publish task가 여기에 생성한다고 가정

# === 1. Publish ===
echo "[DeployServer::PublishProject] publishing project..."
dotnet publish "$PROJECT_PATH" -c Release -o "$PUBLISH_DIR"

if [ $? -ne 0 ]; then
    echo "publish failed."
    exit 1
fi

# === 2. 파일 복사 ===
echo "[DeployServer::CopyFile] copying files to remote server..."
rsync -avz -e "ssh -p $REMOTE_PORT" "$PUBLISH_DIR"/ "$REMOTE_USER@$REMOTE_HOST:$REMOTE_PATH"

if [ $? -ne 0 ]; then
    echo "[DeployServer::CopyFile] rsync failed."
    exit 1
fi

# === 3. 서비스 재시작 ===
echo "[DeployServer::RestartService] Restarting service on remote server..."
ssh -tt -p $REMOTE_PORT $REMOTE_USER@$REMOTE_HOST "sudo systemctl restart $SERVICE_NAME"

if [ $? -eq 0 ]; then
    echo "[DeployServer::RestartService] Deployment complete! Service restarted successfully."
else
    echo "[DeployServer::RestartService] service restart failed."
fi

# === 4. 로그 보기 ===
echo "[DeployServer::ShowLog] Showing latest service logs (Ctrl+C to exit)"
ssh -tt -p $REMOTE_PORT $REMOTE_USER@$REMOTE_HOST "sudo journalctl -u $SERVICE_NAME -n 30 -f"