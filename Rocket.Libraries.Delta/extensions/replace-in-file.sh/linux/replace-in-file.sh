#!/bin/bash
while getopts o:n:f: flag
do
    case "${flag}" in
        o) old=${OPTARG};;
        n) new=${OPTARG};;
        f) file=${OPTARG};;
    esac
done
sed -i "s/$old/$new/g" $file