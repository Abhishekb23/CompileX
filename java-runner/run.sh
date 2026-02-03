#!/bin/sh
set -e

echo "$CODE" > Main.java

javac Main.java
java Main
