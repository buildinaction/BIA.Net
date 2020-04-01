# New Project
This document explains how to create a new project based on the MyCompany Angular framework.<br/>

## Prerequisite

### Knowledge to have:
<ul>
<li><a href="https://angular.io/">Angular</a></li>
<li><a href="https://www.learnrxjs.io/">RXJS</a></li>
<li><a href="https://ngrx.io/">NGRX</a></li>
<li><a href="https://www.primefaces.org/primeng/">PrimeNG component list.</a></li>
</ul>

### Configuration for the MyCompany proxy
<a href="https://www.architectryan.com/2018/08/31/how-to-change-environment-variables-on-windows-10/">Add the following environment variables:</a><br/>
<ul>
<li>HTTP_PROXY: http://10.179.8.30:3128/</li>
<li>HTTPS_PROXY: http://10.179.8.30:3128/</li>
<li>NO_PROXY: tfsdm.eu.labinal.snecma</li>
</ul>

### Git
<a href="https://subscription.packtpub.com/book/programming/9781789530094/9/ch09lvl1sec71/installing-git-for-visual-studio-2019">
Installing Git for Visual Studio
</a>

### Node.js
Install the same version of node.js as the one installed on the SREUBLC535 build server (11.10.0).<br/>
To check the installed version of <a href="https://nodejs.org/en/download/releases/">node.js</a>, use the following command: `node -v`<br/>
### npm
Upgrade to the latest version of npm using `npm install -g npm@latest`<br/>

### angular cli
Upgrade to the latest version of npm using `npm install -g @angular/cli@latest`<br/>

### Visual Studio Code
Install <a href="https://code.visualstudio.com/Download">Visual Studio Code</a> and add the following extensions:
<ul>
<li>danwahlin.angular2-snippets</li>
<li>mikael.angular-beastcode</li>
<li>alexiv.vscode-angular2-files</li>
<li>johnpapa.angular2</li>
<li>ms-vsts.team</li>
<li>kisstkondoros.vscode-codemetrics</li>
<li>msjsdiag.debugger-for-chrome</li>
<li>esbenp.prettier-vscode</li>
<li>ms-vscode.vscode-typescript-tslint-plugin</li>
</ul>

### Git config
To find the path to the <b>.gitconfig</b> file, type the following command:</br>
`git config --list --show-origin`</br>
Open your <b>.gitconfig</b> file and add this configuration:
```
[http "https://site1..../"]
                sslVerify = false
                proxy = ""
[http "https://azure.devops.my-company/"]
                sslVerify = false
                proxy = ""
```


## Create a new project
Retrieve the latest version of the BIATemplate project.<br/>
Copy/Paste the contents of the BIATemplate\Angular folder into the Angular folder of your new project.<br/>
Inside the Angular folder of your new project, run the powershell script `new-angular-project.ps1`.<br/>
For <b>old project name?</b>, type <b>BIATemplate</b><br/>
For <b>new project name?</b>, type the name of your project

## File not to be modified
Some files are part of the Framework and should not be modified.
<ul>
<li>src/app/core/bia-core</li>
<li>src/app/shared/bia-shared</li>
<li>src/assets/bia</li>
<li>src/scss/bia</li>
<li>src/app/features/sites</li>
<li>src/app/features/users</li>
</ul>

## NPM Package
The content of the framework is normally sufficient for the needs of any project. You should never install any other npm package other than those provided by the Framework.<br/>
The component library chosen for this framework is <a href="https://www.primefaces.org/primeng/">PrimeNG</a>. You must use only these components.<br/>
If the content of this framework is not enough, please contact first Jérémie Souques before installing an npm package on your project.

## Design
You can access <a href="https://cgifrance.invisionapp.com/share/6CMNQJLWVQX#/screens/315722051">here</a> to the graphic charter. (password: MyCompany)<br />
If you need to modify the PrimeNG component design, you can modify the following file: src\scss\\_app-custom-theme.scss<br/>
For example you can change the row/cell size of the tables by changing the following `padding` property:
``` scss
p-table {
  td {
    font-weight: 300;
    padding: 0.414em 0.857em !important;
  }
}
```
## NGRX Store
The framework and management of the store is based on this application. You can follow this example for the implementation of your store:<br/>
<a href="https://github.com/avatsaev/angular-contacts-app-example">angular-contacts-app-example</a>

## Before commit
before committing your changes, run the following commands:
<ul>
<li>ng lint: You must have the following message: "All files pass linting".</li>
<li>ng build --aot: You must not get an error message.</li>
</ul>
