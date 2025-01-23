#!/bin/sh

echo "Copy SharedCode"

source_folder="./src/SharedCode/"
target_folder="../ProjectFClient/Assets/01.Scripts/SharedCode/"
cp -rf "$source_folder" "$target_folder"