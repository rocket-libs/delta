FROM openjdk:11
 # Switch to root to install
USER root

RUN apt-get update && apt-get install curl
#RUN apt-get update && apt-get install zip unzip
RUN apt-get update && apt-get install xz-utils

# Let's install flutter stuff
USER root
# Android Tools
ARG ANDROID_SDK_TOOLS="4333796"
ENV ANDROID_SDK_URL="https://dl.google.com/android/repository/sdk-tools-linux-${ANDROID_SDK_TOOLS}.zip"
ENV ANDROID_SDK_ROOT="/usr/local/android"
ENV ANDROID_SDK_ARCHIVE="/tmp/android.zip"
RUN curl --output "${ANDROID_SDK_ARCHIVE}" --url "${ANDROID_SDK_URL}" \
  && unzip -q -d "${ANDROID_SDK_ROOT}" "${ANDROID_SDK_ARCHIVE}" \
  && rm "${ANDROID_SDK_ARCHIVE}"

# Android SDK
RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "tools" \
  "platform-tools" \
  "extras;android;m2repository" \
  "extras;google;m2repository" \
  "patcher;v4" 

# ANDROID 28 --------
ARG ANDROID_SDK_MAJOR=28

RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "platforms;android-${ANDROID_SDK_MAJOR}" 

ARG ANDROID_SDK_MINOR=0
ARG ANDROID_SDK_PATCH=3
ARG ANDROID_SDK_VERSION="${ANDROID_SDK_MAJOR}.${ANDROID_SDK_MINOR}.${ANDROID_SDK_PATCH}"
RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "build-tools;${ANDROID_SDK_VERSION}" 


# ANDROID 29 --------
ARG ANDROID_SDK_MAJOR=29

RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "platforms;android-${ANDROID_SDK_MAJOR}" 

ARG ANDROID_SDK_MINOR=0
ARG ANDROID_SDK_PATCH=3
ARG ANDROID_SDK_VERSION="${ANDROID_SDK_MAJOR}.${ANDROID_SDK_MINOR}.${ANDROID_SDK_PATCH}"
RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "build-tools;${ANDROID_SDK_VERSION}" 

# ANDROID 30 --------
ARG ANDROID_SDK_MAJOR=30

RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "platforms;android-${ANDROID_SDK_MAJOR}" 

ARG ANDROID_SDK_MINOR=0
ARG ANDROID_SDK_PATCH=3
ARG ANDROID_SDK_VERSION="${ANDROID_SDK_MAJOR}.${ANDROID_SDK_MINOR}.${ANDROID_SDK_PATCH}"
RUN yes "y" | ${ANDROID_SDK_ROOT}/tools/bin/sdkmanager "build-tools;${ANDROID_SDK_VERSION}" 


# Flutter
ARG FLUTTER_SDK_CHANNEL="stable"
ARG FLUTTER_SDK_VERSION="2.5.3"
ENV FLUTTER_ROOT="/usr/local/flutter"
ENV FLUTTER_SDK_ARCHIVE="/tmp/flutter.tar.xz"
ENV FLUTTER_SDK_URL="https://storage.googleapis.com/flutter_infra_release/releases/${FLUTTER_SDK_CHANNEL}/linux/flutter_linux_${FLUTTER_SDK_VERSION}-${FLUTTER_SDK_CHANNEL}.tar.xz"
RUN curl --output "${FLUTTER_SDK_ARCHIVE}" --url "${FLUTTER_SDK_URL}" \
  && tar --extract --file="${FLUTTER_SDK_ARCHIVE}" --directory=$(dirname ${FLUTTER_ROOT}) \
  && rm "${FLUTTER_SDK_ARCHIVE}" 

# Dependencies
ENV LANG en_US.UTF-8
RUN apt-get update -y \
# Install basics
  && apt-get install -y --no-install-recommends \
  # zip \
  locales \
  libstdc++6 \
  lib32stdc++6 \
  libglu1-mesa \
  build-essential \
# Clean up image
  && locale-gen en_US ${LANG} \
  && dpkg-reconfigure locales \
  && apt-get autoremove -y \
  && rm -rf /var/lib/apt/lists/* 

RUN yes "y" | ${FLUTTER_ROOT}/bin/flutter doctor --android-licenses \
  && ${FLUTTER_ROOT}/bin/flutter precache && ${FLUTTER_ROOT}/bin/flutter doctor

# Edit path and create access to executables
# Add android executables to path (example: adb avdmanager)
ENV PATH="${PATH}:${ANDROID_SDK_ROOT}/tools/bin:${ANDROID_SDK_ROOT}/platform-tools:${ANDROID_SDK_ROOT}/build-toos/${ANDROID_SDK_VERSION}"
# Add flutter executable to path
ENV PATH="${PATH}:${FLUTTER_ROOT}/bin"
# Make it easy to use other Dart and Pub packages
ENV DART_SDK="${FLUTTER_ROOT}/bin/cache/dart-sdk"
ENV PUB_CACHE=${FLUTTER_ROOT}/.pub-cache
ENV PATH="${PATH}:${DART_SDK}/bin:${PUB_CACHE}/bin"

RUN flutter upgrade
# RUN chown -R jenkins /usr/local/flutter
# RUN chown -R jenkins /usr/local/android

# Switch back to Jenkins for this part of the setup
# USER jenkins

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

# Copy everything else and build
COPY . ./
RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;
RUN dotnet publish -c Release -o out --disable-parallel 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "delta.dll"]
