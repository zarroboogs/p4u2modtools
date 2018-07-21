
$eboot = if ($args[0]) { $args[0] } else { "boot.elf" }
$toc = if ($args[1]) { $args[1] } else { "bddata-toc.txt" }

gc $eboot |
    select-string -pattern "data\/[a-z0-9\/_.]+\.[a-z0-9]+" -allmatches |
    % { $_.matches.value } |
    ? { $_ -notmatch "_appendlist" } `
    > $toc
