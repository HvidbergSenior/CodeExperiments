# Here we clean dependencies for the insight api
# We strive to keep the file idompotent
Write-Output "Start Cleaning...."

# POSTGRES
docker rm -f insight-db 2>&1 > $null
Write-Output "Removed Postgres docker container"
Remove-Item -Path ".pgdata" -Recurse -Force -ErrorAction Ignore
Write-Output "Removed Postgres data"

# BROWSERLESS
docker rm -f insight-browserless 2>&1 > $null
Write-Output "Removed browserless docker container"

Write-Output "Done!"
