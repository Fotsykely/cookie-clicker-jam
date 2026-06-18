# Cookie Clicker Jam

Projet Unity (3 développeurs). Voir [CLAUDE.md](CLAUDE.md) pour les règles d'architecture.

## Setup merge Unity (UnityYAMLMerge / Smart Merge)

Les fichiers Unity (`.prefab`, `.unity`, `.asset`, `.anim`, `.controller`) sont du YAML que git
fusionne mal. Le repo versionne déjà [.gitattributes](.gitattributes) qui les associe à un driver
de merge nommé `unityyamlmerge`.

**Chaque dev doit déclarer ce driver une fois en local** (cette ligne n'est jamais commitée :
git refuse d'exécuter un driver défini via un fichier versionné, et le chemin dépend de ta version
d'Unity et de ton OS) :

```bash
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "'<CHEMIN_UNITYYAMLMERGE>' merge -h -p --force %O %B %A %A"
git config merge.unityyamlmerge.recursive binary
```

Remplace `<CHEMIN_UNITYYAMLMERGE>` par le chemin de l'outil sur ta machine :

- **macOS** : `/Applications/Unity/Hub/Editor/<version>/Unity.app/Contents/Helpers/UnityYAMLMerge`
- **Windows** : `C:\Program Files\Unity\Hub\Editor\<version>\Editor\Data\Tools\UnityYAMLMerge.exe`
- **Linux** : `~/Unity/Hub/Editor/<version>/Editor/Data/Tools/UnityYAMLMerge`

Exemple (macOS, Unity 6000.4.11f1) :

```bash
git config merge.unityyamlmerge.driver "'/Applications/Unity/Hub/Editor/6000.4.11f1/Unity.app/Contents/Helpers/UnityYAMLMerge' merge -h -p --force %O %B %A %A"
```

Après ça, `git merge` / `git rebase` fusionnent automatiquement les fichiers Unity quand c'est
possible et ne laissent des marqueurs de conflit que sur les parties vraiment incompatibles.

> ⚠️ Après tout merge touchant un `.prefab` / `.unity`, **ouvre le projet dans Unity** et vérifie
> qu'il n'y a pas de référence cassée (`Missing` / `{fileID: 0}`) avant de committer.
