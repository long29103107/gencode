# Project `gencode`, create a monolith project 

## Build code
* Step 1: Go to the folder you want to generate (folder contains `.sln`)
- Create folder `Solution Items`
- Create file  `database.dbml` in folder `Solution Items`

* Step 2: Open terminal, go to folder contains file `.sln` in source `gencode`
```
dotnet build
```
* Step 3: On terminal, go to folder contains
```
...\gencode\src\gencode\bin\Debug\{{versionNet}}
```
Example
```
...\gencode\src\gencode\bin\Debug\net7.0
```

* Step 4: Run command, (`{{Path}}` is the folder you want to generate )
```
gencode {{genCodeType}} {{nameModule}} {{Path}}
```
Example: Gen all (api, model, repository, service)
```
gencode all Product "D:\\3. DotNet\\TestGenerate\\"
```
## Gen code type
* All: Gen structure, api, model, repository and service
```
gencode all {{nameModule}} {{Path}}
```

* API: Gen structure and api
```
gencode api {{nameModule}} {{Path}}
```
* Model: Gen structure and model
```
gencode model {{nameModule}} {{Path}}
```
* Repository: Gen structure and repository
```
gencode repository {{nameModule}} {{Path}}
```
* Service: Gen structure and service
```
gencode service {{nameModule}} {{Path}}
```