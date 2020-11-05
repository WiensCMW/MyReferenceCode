# Rename files in folder
dir | rename-item -NewName {$_.name -replace "<old_text>","<new_text>"}