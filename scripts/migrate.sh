#!/bin/bash

echo "Starting migration process..."

# Wait for PostgreSQL to be ready
until pg_isready -h postgres -p 5432 -U postgres; do
  echo "Waiting for PostgreSQL to be ready..."
  sleep 2
done

echo "PostgreSQL is ready. Running migrations..."

# Run migrations
dotnet ef database update --no-build --verbose

if [ $? -eq 0 ]; then
    echo "Migrations completed successfully."
else
    echo "Migration failed!"
    exit 1
fi

echo "Starting the application..."
exec dotnet xAppoinmentSystem.API.dll