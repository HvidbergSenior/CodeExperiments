# Here we define dependencies for the insight api
# We strive to keep the file idompotent

$WorkDir = Resolve-Path ".\"

Write-Output "Starting Dependencies...."

# POSTGRES
Write-Output "Starting postgres db"
docker rm -f insight-db 2>&1 > $null
docker run -d --name insight-db -e POSTGRES_PASSWORD=password -e POSTGRES_USER=postgres -d -p 5432:5432 -v $PWD/.pgdata:/var/lib/postgresql/data -v $PWD/init.sql:/docker-entrypoint-initdb.d/init.sql sibedge/postgres-plv8:15.5-3.2.0 > $null
Write-Output "Started postgres"

# BROWSERLESS
Write-Output "Starting browserless"
docker rm -f insight-browserless 2>&1 > $null
docker run -d --name insight-browserless -p 3100:3000 browserless/chrome:1.60-chrome-stable > $null
Write-Output "Started browserless"

Write-Output "Done!"

# Go back to original work dir
cd $WorkDir