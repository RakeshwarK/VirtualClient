"use strict";(self.webpackChunkvirtualclient=self.webpackChunkvirtualclient||[]).push([[2570],{1875:(e,t,r)=>{r.r(t),r.d(t,{assets:()=>a,contentTitle:()=>o,default:()=>p,frontMatter:()=>i,metadata:()=>l,toc:()=>c});var n=r(5893),s=r(3905);const i={},o="StressAppTest Workload Profiles",l={id:"workloads/stressapptest/stressapptest-profiles",title:"StressAppTest Workload Profiles",description:"The following profiles run customer-representative or benchmarking scenarios using the StressAppTest workload.",source:"@site/docs/workloads/stressapptest/stressapptest-profiles.md",sourceDirName:"workloads/stressapptest",slug:"/workloads/stressapptest/stressapptest-profiles",permalink:"/VirtualClient/docs/workloads/stressapptest/stressapptest-profiles",draft:!1,unlisted:!1,editUrl:"https://github.com/microsoft/VirtualClient/edit/main/website/docs/workloads/stressapptest/stressapptest-profiles.md",tags:[],version:"current",frontMatter:{},sidebar:"tutorialSidebar",previous:{title:"StressAppTest",permalink:"/VirtualClient/docs/workloads/stressapptest/"},next:{title:"SuperBenchmark",permalink:"/VirtualClient/docs/workloads/superbenchmark/"}},a={},c=[{value:"PERF-MEM-STRESSAPPTEST.json",id:"perf-mem-stressapptestjson",level:2}];function d(e){const t={a:"a",br:"br",code:"code",h1:"h1",h2:"h2",li:"li",p:"p",pre:"pre",strong:"strong",table:"table",tbody:"tbody",td:"td",th:"th",thead:"thead",tr:"tr",ul:"ul",...(0,s.ah)(),...e.components};return(0,n.jsxs)(n.Fragment,{children:[(0,n.jsx)(t.h1,{id:"stressapptest-workload-profiles",children:"StressAppTest Workload Profiles"}),"\n",(0,n.jsx)(t.p,{children:"The following profiles run customer-representative or benchmarking scenarios using the StressAppTest workload."}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsx)(t.li,{children:(0,n.jsx)(t.a,{href:"/VirtualClient/docs/workloads/stressapptest/",children:"Workload Details"})}),"\n"]}),"\n",(0,n.jsx)(t.h2,{id:"perf-mem-stressapptestjson",children:"PERF-MEM-STRESSAPPTEST.json"}),"\n",(0,n.jsx)(t.p,{children:"Runs the StressAppTest workload for a specific period of time on the system. This profile is designed to allow the user to run the workload for\nthe purpose of evaluating the performance of the Memory over various periods of time while allowing the user to apply a longer-term stress\nto the system if desired."}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsx)(t.p,{children:(0,n.jsx)(t.strong,{children:"Supported Platform/Architectures"})}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsx)(t.li,{children:"linux-x64"}),"\n",(0,n.jsx)(t.li,{children:"linux-arm64"}),"\n"]}),"\n"]}),"\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsx)(t.p,{children:(0,n.jsx)(t.strong,{children:"Supports Disconnected Scenarios"})}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsxs)(t.li,{children:["Yes. When the StressAppTest package is included in 'packages' directory of the Virtual Client.","\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsxs)(t.li,{children:[(0,n.jsx)(t.a,{href:"/VirtualClient/docs/dependencies/0001-install-vc-packages",children:"Installing VC Packages"}),"."]}),"\n"]}),"\n"]}),"\n"]}),"\n"]}),"\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsxs)(t.p,{children:[(0,n.jsx)(t.strong,{children:"Dependencies"}),"\nThe dependencies defined in the 'Dependencies' section of the profile itself are required in order to run the workload operations effectively."]}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsx)(t.li,{children:"Internet connection."}),"\n"]}),"\n",(0,n.jsx)(t.p,{children:"Additional information on components that exist within the 'Dependencies' section of the profile can be found in the following locations:"}),"\n",(0,n.jsxs)(t.ul,{children:["\n",(0,n.jsx)(t.li,{children:(0,n.jsx)(t.a,{href:"https://microsoft.github.io/VirtualClient/docs/category/dependencies/",children:"Installing Dependencies"})}),"\n"]}),"\n"]}),"\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsxs)(t.p,{children:[(0,n.jsx)(t.strong,{children:"Profile Parameters"}),"\nThe following parameters can be optionally supplied on the command line to modify the behaviors of the workload."]}),"\n",(0,n.jsxs)(t.table,{children:[(0,n.jsx)(t.thead,{children:(0,n.jsxs)(t.tr,{children:[(0,n.jsx)(t.th,{children:"Parameter"}),(0,n.jsx)(t.th,{children:"Purpose"}),(0,n.jsx)(t.th,{children:"Acceptable Range"}),(0,n.jsx)(t.th,{children:"Default Value"})]})}),(0,n.jsxs)(t.tbody,{children:[(0,n.jsxs)(t.tr,{children:[(0,n.jsx)(t.td,{children:"TimeInSeconds"}),(0,n.jsx)(t.td,{children:"Time (in seconds) to run StressAppTest Memory Workload"}),(0,n.jsx)(t.td,{children:">0"}),(0,n.jsx)(t.td,{children:"60"})]}),(0,n.jsxs)(t.tr,{children:[(0,n.jsx)(t.td,{children:"UseCpuStressfulMemoryCopy"}),(0,n.jsx)(t.td,{children:"Switch to toggle StressAppTest built-in CPU Stressful Memory Copy option. If enabled (true), CPU Usage should increase"}),(0,n.jsx)(t.td,{children:"true/false"}),(0,n.jsx)(t.td,{children:"false"})]})]})]}),"\n"]}),"\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsxs)(t.p,{children:[(0,n.jsx)(t.strong,{children:"Profile Runtimes"}),(0,n.jsx)(t.br,{}),"\n","See the 'Metadata' section of the profile for estimated runtimes. These timings represent the length of time required to run a single round of profile\nactions. These timings can be used to determine minimum required runtimes for the Virtual Client in order to get results. These are often estimates based on the\nnumber of system cores."]}),"\n"]}),"\n",(0,n.jsxs)(t.li,{children:["\n",(0,n.jsxs)(t.p,{children:[(0,n.jsx)(t.strong,{children:"Usage Examples"}),"\nThe following section provides a few basic examples of how to use the workload profile."]}),"\n",(0,n.jsx)(t.pre,{children:(0,n.jsx)(t.code,{className:"language-bash",children:'# Execute the workload profile\n./VirtualClient --profile=PERF-MEM-STRESSAPPTEST.json --system=Demo --timeout=90 --packageStore="{BlobConnectionString|SAS Uri}"\n\n# Override the default parameters to run the workload for a longer period of time\n./VirtualClient --profile=PERF-MEM-STRESSAPPTEST.json --system=Demo --timeout=90 --packageStore="{BlobConnectionString|SAS Uri}" --parameters="TimeInSeconds=240"\n\n# Override the default parameters to change the "torture settings" when running the workload.\n./VirtualClient --profile=PERF-MEM-STRESSAPPTEST.json --system=Demo --timeout=90 --packageStore="{BlobConnectionString|SAS Uri}" --parameters="TimeInSeconds=240,,,UseCpuStressfulMemoryCopy=true"\n'})}),"\n"]}),"\n"]})]})}function p(e={}){const{wrapper:t}={...(0,s.ah)(),...e.components};return t?(0,n.jsx)(t,{...e,children:(0,n.jsx)(d,{...e})}):d(e)}},3905:(e,t,r)=>{r.d(t,{ah:()=>c});var n=r(7294);function s(e,t,r){return t in e?Object.defineProperty(e,t,{value:r,enumerable:!0,configurable:!0,writable:!0}):e[t]=r,e}function i(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);t&&(n=n.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,n)}return r}function o(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?i(Object(r),!0).forEach((function(t){s(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):i(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function l(e,t){if(null==e)return{};var r,n,s=function(e,t){if(null==e)return{};var r,n,s={},i=Object.keys(e);for(n=0;n<i.length;n++)r=i[n],t.indexOf(r)>=0||(s[r]=e[r]);return s}(e,t);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);for(n=0;n<i.length;n++)r=i[n],t.indexOf(r)>=0||Object.prototype.propertyIsEnumerable.call(e,r)&&(s[r]=e[r])}return s}var a=n.createContext({}),c=function(e){var t=n.useContext(a),r=t;return e&&(r="function"==typeof e?e(t):o(o({},t),e)),r},d={inlineCode:"code",wrapper:function(e){var t=e.children;return n.createElement(n.Fragment,{},t)}},p=n.forwardRef((function(e,t){var r=e.components,s=e.mdxType,i=e.originalType,a=e.parentName,p=l(e,["components","mdxType","originalType","parentName"]),h=c(r),u=s,f=h["".concat(a,".").concat(u)]||h[u]||d[u]||i;return r?n.createElement(f,o(o({ref:t},p),{},{components:r})):n.createElement(f,o({ref:t},p))}));p.displayName="MDXCreateElement"}}]);