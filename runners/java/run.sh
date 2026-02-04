#!/bin/sh
set -e

if [ -z "$MAIN_CLASS" ]; then
  echo "Error: MAIN_CLASS not provided"
  exit 1
fi

# Compile all Java files
javac *.java

# Run the detected main class
java -cp . "$MAIN_CLASS"
