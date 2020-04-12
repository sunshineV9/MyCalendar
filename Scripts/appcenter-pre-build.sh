#!/usr/bin/env bash

# Updating Info.plist

PLIST=$BUILD_REPOSITORY_LOCALPATH/MyApp/iOS/Info.plist

/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString 1.0.${APPCENTER_BUILD_ID}" $PLIST

# Print out file for reference
cat $PLIST

echo "Updated info.plist!"

#!/usr/bin/env bash

# Updating Manifest

MANIFEST="$BUILD_REPOSITORY_LOCALPATH/MyApp/Android/Properties/AndroidManifest.xml"

VERSIONNAME=`grep versionName ${MANIFEST} | sed 's/.*versionName\s*=\s*\"\([^\"]*\)\".*/\1/g'`

sed -i.bak "s/android:versionName="\"${VERSIONNAME}\""/android:versionName="\"1.0.${APPCENTER_BUILD_ID}\""/" ${MANIFEST}

rm -f ${MANIFEST}.bak

# Print out file for reference
cat $MANIFEST

echo "updated AndroidManifest.xml"