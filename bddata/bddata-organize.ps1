
$target = if ($args[0]) { $args[0] } else { "bddata-toc.txt" }

$files = gci "bddata+*"
$content = gc $target

# make dirs
$dirs = Split-Path $content -Parent | group

foreach ($dir in $dirs.Name) {
    if ($dir -and !(Test-Path $dir)) {
        mkdir $dir -Verbose
    }
}

# move files
for ($i=0; $i -lt $files.Count; $i++) {
    move-item $files[$i] $content[$i] -Verbose
}
