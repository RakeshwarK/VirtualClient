"use strict";(self.webpackChunkvirtualclient=self.webpackChunkvirtualclient||[]).push([[8644],{7839:(e,n,t)=>{t.r(n),t.d(n,{assets:()=>s,contentTitle:()=>l,default:()=>u,frontMatter:()=>o,metadata:()=>c,toc:()=>a});var i=t(5893),r=t(3905);const o={},l="Building Docker Containers",c={id:"developing/0061-build-docker-container",title:"Building Docker Containers",description:"The following sections cover how to build a Docker image that will contain the Virtual Client.",source:"@site/docs/developing/0061-build-docker-container.md",sourceDirName:"developing",slug:"/developing/0061-build-docker-container",permalink:"/VirtualClient/docs/developing/0061-build-docker-container",draft:!1,unlisted:!1,editUrl:"https://github.com/microsoft/VirtualClient/edit/main/website/docs/developing/0061-build-docker-container.md",tags:[],version:"current",frontMatter:{},sidebar:"tutorialSidebar",previous:{title:"Workload Selection Recommendations",permalink:"/VirtualClient/docs/developing/0060-workload-recommendations"},next:{title:"Error Handling Recommendations",permalink:"/VirtualClient/docs/developing/0070-error-handling"}},s={},a=[{value:"Installing Docker on your Windows Dev machine",id:"installing-docker-on-your-windows-dev-machine",level:2},{value:"Building VC Container",id:"building-vc-container",level:2},{value:"Command to build",id:"command-to-build",level:3},{value:"Switch between Windows and Linux",id:"switch-between-windows-and-linux",level:3},{value:"Supported Platforms",id:"supported-platforms",level:3},{value:"Run VC Container",id:"run-vc-container",level:2}];function d(e){const n={a:"a",code:"code",del:"del",h1:"h1",h2:"h2",h3:"h3",li:"li",ol:"ol",p:"p",pre:"pre",strong:"strong",ul:"ul",...(0,r.ah)(),...e.components};return(0,i.jsxs)(i.Fragment,{children:[(0,i.jsx)(n.h1,{id:"building-docker-containers",children:"Building Docker Containers"}),"\n",(0,i.jsx)(n.p,{children:"The following sections cover how to build a Docker image that will contain the Virtual Client."}),"\n",(0,i.jsx)(n.h2,{id:"installing-docker-on-your-windows-dev-machine",children:"Installing Docker on your Windows Dev machine"}),"\n",(0,i.jsx)(n.p,{children:"To run Docker on your presumeably Windows Dev Machine, you need to install both Docker and WSL. Reboots might be required."}),"\n",(0,i.jsxs)(n.ol,{children:["\n",(0,i.jsx)(n.li,{children:(0,i.jsx)(n.a,{href:"https://hub.docker.com/editions/community/docker-ce-desktop-windows",children:"Docker desktop for Windows"})}),"\n",(0,i.jsx)(n.li,{children:(0,i.jsx)(n.a,{href:"https://docs.microsoft.com/en-us/windows/wsl/install-win10#manual-installation-steps",children:"Install Windows Subsystem for Linux"})}),"\n"]}),"\n",(0,i.jsx)(n.h2,{id:"building-vc-container",children:"Building VC Container"}),"\n",(0,i.jsxs)(n.p,{children:["VC containers uses ",(0,i.jsx)(n.a,{href:"https://hub.docker.com/_/microsoft-dotnet-runtime/",children:"official .NET Runtime docker images"})," as base images. Specific tags are used match the OS/Architecture for the container.\nFor example, mcr.microsoft.com/dotnet/runtime:5.0.9-focal-arm64v8 will pull up the Ubuntu20.04-ARM64 image with .NET5.0.9. Please refer to the Official .NET Docker repository page if tags need to be updated in Dockerfile."]}),"\n",(0,i.jsx)(n.h3,{id:"command-to-build",children:"Command to build"}),"\n",(0,i.jsxs)(n.p,{children:["The command to build docker container is ",(0,i.jsx)(n.a,{href:"https://docs.docker.com/engine/reference/commandline/build/",children:'"docker build"'}),"."]}),"\n",(0,i.jsxs)(n.p,{children:[(0,i.jsx)(n.strong,{children:"IMPORTANT"}),': Always tag your docker container using "-t" and ":", "-t test-win-x64:1.0.1.1" will mark "name=test-win-x64" and "tag=1.0.1.1".\nIf tag after column sign is not supplied, it will be assigned a default tag "latest", which could overwrite production image if pushed accidentally.']}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{children:"$ docker build -f src\\VirtualClient\\VirtualClient.Packaging\\dockerfiles\\win-x64.dockerfile -t test-win-x64:1.0.1.1 E:\\source\\VirtualClient\n$ docker build -f src\\VirtualClient\\VirtualClient.Packaging\\dockerfiles\\linux-x64.dockerfile -t test-linux-x64:1.0.1.1 E:\\source\\VirtualClient\n"})}),"\n",(0,i.jsx)(n.h3,{id:"switch-between-windows-and-linux",children:(0,i.jsx)(n.a,{href:"https://docs.microsoft.com/en-us/windows/wsl/install-win10#manual-installation-steps",children:"Switch between Windows and Linux"})}),"\n",(0,i.jsx)(n.p,{children:"The container can only run as the Host operating system. On your Windows dev machine, Docker runs Linux containers on the WSL backend, and runs Windows containers on your host operating system.\nAs a result, the docker CLI needs to switch mode if you want to test Windows containers and then Linux containers.\nFrom the Docker Desktop menu, you can toggle which daemon (Linux or Windows) the Docker CLI talks to.\nSelect Switch to Windows containers to use Windows containers, or select Switch to Linux containers to use Linux containers (the default)."}),"\n",(0,i.jsx)(n.h3,{id:"supported-platforms",children:"Supported Platforms"}),"\n",(0,i.jsxs)(n.ul,{children:["\n",(0,i.jsx)(n.li,{children:"linux-x64"}),"\n",(0,i.jsx)(n.li,{children:"linux-arm64"}),"\n",(0,i.jsx)(n.li,{children:"win-x64"}),"\n",(0,i.jsxs)(n.li,{children:[(0,i.jsx)(n.del,{children:"win-arm64"})," (Working to add support)"]}),"\n"]}),"\n",(0,i.jsx)(n.p,{children:"It is important to know that Container shares the OS with the host, so sometimes the container will not be able to run on your host even though both are running as win-x64.\nWindowsServerCore2022-Container is not able to run on Windows10-Host for example."}),"\n",(0,i.jsx)(n.h2,{id:"run-vc-container",children:"Run VC Container"}),"\n",(0,i.jsxs)(n.p,{children:["The command to run docker container is ",(0,i.jsx)(n.a,{href:"https://docs.docker.com/engine/reference/commandline/run/",children:'"docker run"'})]}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{children:"$ docker run -d -p 3000:80 test-win-x64:1.0.1.1 C:\\VirtualClient\\VirtualClient.exe <Your VC Command>\n$ docker run -d -p 3000:80 test-linux-x64:1.0.1.1 VirtualClient/VirtualClient <Your VC Command>\n"})})]})}function u(e={}){const{wrapper:n}={...(0,r.ah)(),...e.components};return n?(0,i.jsx)(n,{...e,children:(0,i.jsx)(d,{...e})}):d(e)}},3905:(e,n,t)=>{t.d(n,{ah:()=>a});var i=t(7294);function r(e,n,t){return n in e?Object.defineProperty(e,n,{value:t,enumerable:!0,configurable:!0,writable:!0}):e[n]=t,e}function o(e,n){var t=Object.keys(e);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);n&&(i=i.filter((function(n){return Object.getOwnPropertyDescriptor(e,n).enumerable}))),t.push.apply(t,i)}return t}function l(e){for(var n=1;n<arguments.length;n++){var t=null!=arguments[n]?arguments[n]:{};n%2?o(Object(t),!0).forEach((function(n){r(e,n,t[n])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(t)):o(Object(t)).forEach((function(n){Object.defineProperty(e,n,Object.getOwnPropertyDescriptor(t,n))}))}return e}function c(e,n){if(null==e)return{};var t,i,r=function(e,n){if(null==e)return{};var t,i,r={},o=Object.keys(e);for(i=0;i<o.length;i++)t=o[i],n.indexOf(t)>=0||(r[t]=e[t]);return r}(e,n);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);for(i=0;i<o.length;i++)t=o[i],n.indexOf(t)>=0||Object.prototype.propertyIsEnumerable.call(e,t)&&(r[t]=e[t])}return r}var s=i.createContext({}),a=function(e){var n=i.useContext(s),t=n;return e&&(t="function"==typeof e?e(n):l(l({},n),e)),t},d={inlineCode:"code",wrapper:function(e){var n=e.children;return i.createElement(i.Fragment,{},n)}},u=i.forwardRef((function(e,n){var t=e.components,r=e.mdxType,o=e.originalType,s=e.parentName,u=c(e,["components","mdxType","originalType","parentName"]),h=a(t),m=r,p=h["".concat(s,".").concat(m)]||h[m]||d[m]||o;return t?i.createElement(p,l(l({ref:n},u),{},{components:t})):i.createElement(p,l({ref:n},u))}));u.displayName="MDXCreateElement"}}]);