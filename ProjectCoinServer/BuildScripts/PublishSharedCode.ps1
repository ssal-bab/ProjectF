Write-Output "Copy SharedCode"

$source_folder = "./src/SharedCode/"
$target_folder = "../ProjectCoinClient/Assets/01.Scripts/SharedCode/"

# 폴더 복사
Copy-Item -Path $source_folder -Destination $target_folder -Recurse -Force
