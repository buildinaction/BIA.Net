# Prepare Migration

## Set tags on project BIATEMPLATE
1. To have the full log history open powersheel:
```ps
cd ...\BIATemplate
git log --pretty=format:"%h - %an, %ad"
```
2. select the correct commit id corresponding to the date 
3. Create the tag directly in the azure devops interface:
https://azure.devops.safran/SafranElectricalAndPower/Digital%20Manufacturing/_git/BIATemplate/tags
4. Sync your local repository BIATemplate
5. Create the git differential patch with git batch (V2.30.1 or higher) ex for V3.2.0 to V3.2.2:
```ps
cd ...\\BIATemplate
git diff V3.2.0 -- Angular V3.2.2 -- Angular > Migration\\Angular-3.2.0-3.2.2.patch
```
6. this patch can be apply like this
```ps
cd ...\\YourProject
git apply --reject --whitespace=fix "D:\Sources\Azure.DevOps.Safran\DigitalManufacturing\BIATemplate\Migration\Angular-3.2.0-3.2.2.patch"
```