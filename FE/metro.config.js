const { getDefaultConfig } = require('expo/metro-config');
const path = require('path');

const config = getDefaultConfig(__dirname);

// Exclude Android build intermediates from Metro file watcher
config.resolver.blockList = [
  /node_modules\/.*\/android\/build\/.*/,
  /node_modules\/.*\/android\/\.cxx\/.*/,
  /android\/build\/.*/,
  /android\/app\/build\/.*/,
  /android\/app\/\.cxx\/.*/,
];

module.exports = config;
