# FRAMEWORK

## Customize PrimeNG Theme
The PrimeNG theme chosen for this framework is the <a href="https://www.primefaces.org/ultima-ng/">Ultima theme</a>.
In the BIADemo project, the content of the theme can be found in the following folders :<br/>
src/assets/layout<br/>
src/assets/sass<br/>
src/assets/theme<br/>
To be able to customize the theme, you must first install node-sass globally with the following command: <b>npm install -g node-sass</b>.<br/>
You only must change the <b>src/assets/sass/overrides</b> folder.<br/>
Once the changes have been made, open a cmd. Position yourself on the <b>Angular</b> folder of your project. Then, launch the following command to generate the css.<br/>
``` cmd
node-sass .\src\assets\theme\theme-primeng-dark-my-company.scss .\src\assets\theme\theme-primeng-dark-my-company.css && node-sass .\src\assets\layout\css\layout-primeng-dark-my-company.scss .\src\assets\layout\css\layout-primeng-dark-my-company.css && node-sass .\src\assets\theme\theme-primeng-light-my-company.scss .\src\assets\theme\theme-primeng-light-my-company.css && node-sass .\src\assets\layout\css\layout-primeng-light-my-company.scss .\src\assets\layout\css\layout-primeng-light-my-company.css
```