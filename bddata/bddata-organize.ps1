
$target = if ($args[0]) { $args[0] } else { "bddata-toc.txt" }

$files = gci "bddata+*"

# make dirs
$dirs = Split-Path $target -Parent | group

foreach ($dir in $dirs.Name) {
    if (!(Test-Path $dir)) {
        mkdir $dir -Verbose
    }
}

# move files
for ($i=0; $i -lt $files.Count; $i++) {
    move-item $files[$i] $target[$i] -Verbose
}
