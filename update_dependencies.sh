#!/usr/bin/env bash

echo "Update tool..."
if dotnet tool update --global dotnet-outdated-tool; then
	echo "Successfully updated"
else
	dotnet tool install --global dotnet-outdated-tool
fi

echo "Upgrade outdated packages..."
dotnet outdated -u NetSentinel.sln

echo "Finished"

