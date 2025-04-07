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
echo "publishing project..."
dotnet publish "$PROJECT_PATH" -c Release -o "$PUBLISH_DIR"

if [ $? -ne 0 ]; then
    echo "publish failed."
    exit 1
fi

# === 2. 파일 복사 ===
echo "copying files to remote server..."
rsync -avz -e "ssh -p $REMOTE_PORT" "$PUBLISH_DIR"/ "$REMOTE_USER@$REMOTE_HOST:$REMOTE_PATH"

if [ $? -ne 0 ]; then
    echo "rsync failed."
    exit 1
fi

# === 3. 서비스 재시작 ===
echo "Restarting service on remote server..."
ssh -tt -p $REMOTE_PORT $REMOTE_USER@$REMOTE_HOST "sudo systemctl restart $SERVICE_NAME"

if [ $? -eq 0 ]; then
    echo "Deployment complete! Service restarted successfully."
else
    echo "서비스 재시작 실패. 서버에서 로그를 확인하세요."
fi

# === 4. 로그 보기 ===
echo "Showing latest service logs (Ctrl+C to exit)"
ssh -tt -p $REMOTE_PORT $REMOTE_USER@$REMOTE_HOST "sudo journalctl -u $SERVICE_NAME -n 30 -f"