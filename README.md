Simple reverse tcp payload with dolphin

=======================================
|            Cool prompt              |
=======================================

{
  "$schema": "https://raw.githubusercontent.com/JanDeDobbeleer/oh-my-posh/main/themes/schema.json",
  "Output": "",
  "blocks": [
    {
      "alignment": "left",
      "segments": [
        {
          "foreground": "#ff5252",
          "properties": {
            "template": "root "
          },
          "style": "plain",
          "type": "root"
        },
        {
          "foreground": "#44cbb2",
          "properties": {
            "template": "{{ if .SSHSession }}\uf817 {{ end }}{{ .UserName }}@{{ .HostName }}:"
          },
          "style": "plain",
          "type": "session"
        },
        {
          "foreground": "#277efc",
          "properties": {
            "style": "full",
            "template": "{{ .Path }}"
          },
          "style": "plain",
          "type": "path"
        },
        {
          "foreground": "#C1C106",
          "properties": {
            "template": "<#ffffff>git:</>{{ .HEAD }} {{ .BranchStatus }}{{ if .Working.Changed }} \uf044 {{ .Working.String }}{{ end }}{{ if and (.Staging.Changed) (.Working.Changed) }} |{{ end }}{{ if .Staging.Changed }} \uf046 {{ .Staging.String }}{{ end }}{{ if gt .StashCount 0}} \uf692 {{ .StashCount }}{{ end }}{{ if gt .WorktreeCount 0}} \uf1bb {{ .WorktreeCount }}{{ end }} "
          },
          "style": "plain",
          "type": "git"
        },
        {
          "foreground": "#100e23",
          "properties": {
            "template": " \ue235 {{ if .Error }}{{ .Error }}{{ else }}{{ if .Venv }}{{ .Venv }} {{ end }}{{ .Full }}{{ end }} "
          },
          "style": "plain",
          "type": "python"
        },
        {
          "foreground": "#e7eaec",
          "properties": {
            "template": "$ "
          },
          "style": "plain",
          "type": "text"
        }
      ],
      "type": "prompt"
    }
  ],
  "version": 1
}
