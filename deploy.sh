#!/bin/bash

# LolaCredits - Quick Deploy Script (Linux/Mac)
# Run this script to deploy locally with Docker

echo "🚀 LolaCredits Deployment Script"
echo "================================="
echo ""

# Check if Docker is installed
echo "Checking Docker..."
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

echo "✅ Docker is installed"

# Check if .env exists
if [ ! -f ".env" ]; then
    echo ""
    echo "Creating .env file from template..."
    cp .env.example .env
    echo "✅ .env file created. Please edit it with your configuration."
    echo ""
    echo "Edit .env file and run this script again."
    exit 0
fi

# Stop existing containers
echo ""
echo "Stopping existing containers..."
docker-compose down

# Build and start containers
echo ""
echo "Building and starting containers..."
docker-compose up --build -d

# Wait for services to be ready
echo ""
echo "Waiting for services to start..."
sleep 10

# Check container status
echo ""
echo "Container Status:"
docker-compose ps

echo ""
echo "================================="
echo "✅ Deployment Complete!"
echo ""
echo "🌐 Frontend: http://localhost"
echo "🔧 Backend API: http://localhost:5228/swagger"
echo ""
echo "To view logs: docker-compose logs -f"
echo "To stop: docker-compose down"
echo ""
