"use strict";(self.webpackChunkvirtualclient=self.webpackChunkvirtualclient||[]).push([[2173],{7757:(e,r,s)=>{s.r(r),s.d(r,{assets:()=>l,contentTitle:()=>a,default:()=>h,frontMatter:()=>t,metadata:()=>o,toc:()=>c});var n=s(5893),i=s(3905);const t={},a="7zip",o={id:"workloads/compression/7zip",title:"7zip",description:"7-Zip is a file archiver with a high compression ratio.",source:"@site/docs/workloads/compression/7zip.md",sourceDirName:"workloads/compression",slug:"/workloads/compression/7zip",permalink:"/VirtualClient/docs/workloads/compression/7zip",draft:!1,unlisted:!1,editUrl:"https://github.com/microsoft/VirtualClient/edit/main/website/docs/workloads/compression/7zip.md",tags:[],version:"current",frontMatter:{},sidebar:"tutorialSidebar",previous:{title:"Compression/Decompression Workloads Profiles",permalink:"/VirtualClient/docs/workloads/compression/compression-profiles"},next:{title:"Gzip",permalink:"/VirtualClient/docs/workloads/compression/gzip"}},l={},c=[{value:"Documentation",id:"documentation",level:3},{value:"Supported Platforms and Architectures",id:"supported-platforms-and-architectures",level:3},{value:"Package Dependencies",id:"package-dependencies",level:3},{value:"Workload Usage",id:"workload-usage",level:3},{value:"What is Being Tested?",id:"what-is-being-tested",level:3}];function d(e){const r={a:"a",h1:"h1",h3:"h3",li:"li",p:"p",table:"table",tbody:"tbody",td:"td",th:"th",thead:"thead",tr:"tr",ul:"ul",...(0,i.ah)(),...e.components};return(0,n.jsxs)(n.Fragment,{children:[(0,n.jsx)(r.h1,{id:"7zip",children:"7zip"}),"\n",(0,n.jsx)(r.p,{children:"7-Zip is a file archiver with a high compression ratio."}),"\n",(0,n.jsx)(r.p,{children:"The main features of 7-Zip are:"}),"\n",(0,n.jsxs)(r.ul,{children:["\n",(0,n.jsx)(r.li,{children:"High compression ratio in 7z format with LZMA and LZMA2 compression"}),"\n",(0,n.jsx)(r.li,{children:"Supported formats:"}),"\n",(0,n.jsx)(r.li,{children:"Packing / unpacking: 7z, XZ, BZIP2, GZIP, TAR, ZIP and WIM"}),"\n",(0,n.jsx)(r.li,{children:"Unpacking only: APFS, AR, ARJ, CAB, CHM, CPIO, CramFS, DMG, EXT, FAT, GPT, HFS, IHEX, ISO, LZH, LZMA, MBR, MSI, NSIS, NTFS, QCOW2, RAR, RPM, SquashFS, UDF, UEFI, VDI, VHD, VHDX, VMDK, WIM, XAR and Z."}),"\n",(0,n.jsx)(r.li,{children:"For ZIP and GZIP formats, 7-Zip provides a compression ratio that is 2-10 % better than the ratio provided by PKZip and WinZip"}),"\n",(0,n.jsx)(r.li,{children:"Strong AES-256 encryption in 7z and ZIP formats"}),"\n",(0,n.jsx)(r.li,{children:"Self-extracting capability for 7z format"}),"\n",(0,n.jsx)(r.li,{children:"Integration with Windows Shell"}),"\n",(0,n.jsx)(r.li,{children:"Powerful File Manager"}),"\n",(0,n.jsx)(r.li,{children:"Powerful command line version"}),"\n",(0,n.jsx)(r.li,{children:"Plugin for FAR Manager"}),"\n",(0,n.jsx)(r.li,{children:"Localizations for 87 languages"}),"\n"]}),"\n",(0,n.jsx)(r.h3,{id:"documentation",children:"Documentation"}),"\n",(0,n.jsxs)(r.ul,{children:["\n",(0,n.jsx)(r.li,{children:(0,n.jsx)(r.a,{href:"https://www.7-zip.org/",children:"7zip"})}),"\n"]}),"\n",(0,n.jsx)(r.h3,{id:"supported-platforms-and-architectures",children:"Supported Platforms and Architectures"}),"\n",(0,n.jsxs)(r.ul,{children:["\n",(0,n.jsx)(r.li,{children:"win-x64"}),"\n",(0,n.jsx)(r.li,{children:"win-arm64"}),"\n"]}),"\n",(0,n.jsx)(r.h3,{id:"package-dependencies",children:"Package Dependencies"}),"\n",(0,n.jsx)(r.p,{children:"The following package dependencies are required to be installed on the Windows system in order to support the requirements\nof the 7zip workload. Note that the Virtual Client will handle the installation of any required dependencies."}),"\n",(0,n.jsxs)(r.ul,{children:["\n",(0,n.jsx)(r.li,{children:"7zip.commandline"}),"\n",(0,n.jsx)(r.li,{children:"unzip"}),"\n",(0,n.jsx)(r.li,{children:"wget"}),"\n"]}),"\n",(0,n.jsx)(r.h3,{id:"workload-usage",children:"Workload Usage"}),"\n",(0,n.jsx)(r.p,{children:"7z  [switches...] [file_names...] [@listfile]"}),"\n",(0,n.jsxs)(r.p,{children:["a : Add files to archive ",(0,n.jsx)("br",{}),"\nb : Benchmark ",(0,n.jsx)("br",{}),"\nd : Delete files from archive ",(0,n.jsx)("br",{}),"\ne : Extract files from archive (without using directory names) ",(0,n.jsx)("br",{}),"\nh : Calculate hash values for files ",(0,n.jsx)("br",{}),"\ni : Show information about supported formats ",(0,n.jsx)("br",{}),"\nl : List contents of archive ",(0,n.jsx)("br",{}),"\nrn : Rename files in archive ",(0,n.jsx)("br",{}),"\nt : Test integrity of archive ",(0,n.jsx)("br",{}),"\nu : Update files to archive ",(0,n.jsx)("br",{}),"\nx : eXtract files with full paths ",(0,n.jsx)("br",{})]}),"\n",(0,n.jsxs)(r.p,{children:["-- : Stop switches and @listfile parsing ",(0,n.jsx)("br",{}),"\n-ai[r[-|0]](@listfile|!wildcard) : Include archives ",(0,n.jsx)("br",{}),"\n-ax",(0,n.jsx)(r.a,{href:"@listfile%7C!wildcard",children:"r[-|0]"})," : eXclude archives ",(0,n.jsx)("br",{}),"\n-ao(a|s|t|u) : set Overwrite mode ",(0,n.jsx)("br",{}),"\n-an : disable archive_name field ",(0,n.jsx)("br",{}),"\n-bb[0-3] : set output log level ",(0,n.jsx)("br",{}),"\n-bd : disable progress indicator ",(0,n.jsx)("br",{}),"\n-bs(o|e|p)(0|1|2) : set output stream for output/error/progress line ",(0,n.jsx)("br",{}),"\n-bt : show execution time statistics ",(0,n.jsx)("br",{}),"\n-i",(0,n.jsx)(r.a,{href:"@listfile%7C!wildcard",children:"r[-|0]"})," : Include filenames ",(0,n.jsx)("br",{}),"\n-m(Parameters) : set compression Method ",(0,n.jsx)("br",{}),"\n-mmt[N] : set number of CPU threads ",(0,n.jsx)("br",{}),"\n-mx[N] : set compression level: -mx1 (fastest) ... -mx9 (ultra) ",(0,n.jsx)("br",{}),"\n-o(Directory) : set Output directory ",(0,n.jsx)("br",{}),"\n-p(Password) : set Password ",(0,n.jsx)("br",{}),"\n-r[-|0] : Recurse subdirectories ",(0,n.jsx)("br",{}),"\n-sa(a|e|s) : set Archive name mode ",(0,n.jsx)("br",{}),"\n-scc(UTF-8|WIN|DOS) : set charset for console input/output ",(0,n.jsx)("br",{}),"\n-scs(UTF-8|UTF-16LE|UTF-16BE|WIN|DOS|(id)) : set charset for list files ",(0,n.jsx)("br",{}),"\n-scrc[CRC32|CRC64|SHA1|SHA256|*] : set hash function for x, e, h commands ",(0,n.jsx)("br",{}),"\n-sdel : delete files after compression ",(0,n.jsx)("br",{}),"\n-seml[.] : send archive by email ",(0,n.jsx)("br",{}),"\n-sfx[(name)] : Create SFX archive ",(0,n.jsx)("br",{}),"\n-si[(name)] : read data from stdin ",(0,n.jsx)("br",{}),"\n-slp : set Large Pages mode ",(0,n.jsx)("br",{}),"\n-slt : show technical information for l (List) command ",(0,n.jsx)("br",{}),"\n-snh : store hard links as links ",(0,n.jsx)("br",{}),"\n-snl : store symbolic links as links ",(0,n.jsx)("br",{}),"\n-sni : store NT security information ",(0,n.jsx)("br",{}),"\n-sns[-] : store NTFS alternate streams ",(0,n.jsx)("br",{}),"\n-so : write data to stdout ",(0,n.jsx)("br",{}),"\n-spd : disable wildcard matching for file names ",(0,n.jsx)("br",{}),"\n-spe : eliminate duplication of root folder for extract command ",(0,n.jsx)("br",{}),"\n-spf : use fully qualified file paths ",(0,n.jsx)("br",{}),"\n-ssc[-] : set sensitive case mode ",(0,n.jsx)("br",{}),"\n-sse : stop archive creating, if it can't open some input file ",(0,n.jsx)("br",{}),"\n-ssw : compress shared files ",(0,n.jsx)("br",{}),"\n-stl : set archive timestamp from the most recently modified file ",(0,n.jsx)("br",{}),"\n-stm(HexMask) : set CPU thread affinity mask (hexadecimal number) ",(0,n.jsx)("br",{}),"\n-stx(Type) : exclude archive type ",(0,n.jsx)("br",{}),"\n-t(Type) : Set type of archive ",(0,n.jsx)("br",{}),"\n-u[-][p#][q#][r#][x#][y#][z#][!newArchiveName] : Update options ",(0,n.jsx)("br",{}),"\n-v(Size)[b|k|m|g] : Create volumes ",(0,n.jsx)("br",{}),"\n-w[(path)] : assign Work directory. Empty path means a temporary directory ",(0,n.jsx)("br",{}),"\n-x",(0,n.jsx)(r.a,{href:"@listfile%7C!wildcard",children:"r[-|0]"})," : eXclude filenames ",(0,n.jsx)("br",{}),"\n-y : assume Yes on all queries"]}),"\n",(0,n.jsx)(r.h3,{id:"what-is-being-tested",children:"What is Being Tested?"}),"\n",(0,n.jsx)(r.p,{children:"7zip is used to measure performance in terms of compressionTime, and ratio of compressed size and original size. Below are the metrics measured by 7zip Workload."}),"\n",(0,n.jsxs)(r.table,{children:[(0,n.jsx)(r.thead,{children:(0,n.jsxs)(r.tr,{children:[(0,n.jsx)(r.th,{children:"Name"}),(0,n.jsx)(r.th,{children:"Unit"})]})}),(0,n.jsxs)(r.tbody,{children:[(0,n.jsxs)(r.tr,{children:[(0,n.jsx)(r.td,{children:"Compressed size and Original size ratio"}),(0,n.jsx)(r.td,{children:"-"})]}),(0,n.jsxs)(r.tr,{children:[(0,n.jsx)(r.td,{children:"CompressionTime"}),(0,n.jsx)(r.td,{children:"seconds"})]})]})]}),"\n",(0,n.jsx)(r.h1,{id:"references",children:"References"}),"\n",(0,n.jsxs)(r.ul,{children:["\n",(0,n.jsx)(r.li,{children:(0,n.jsx)(r.a,{href:"https://www.7-zip.org/",children:"7zip"})}),"\n"]})]})}function h(e={}){const{wrapper:r}={...(0,i.ah)(),...e.components};return r?(0,n.jsx)(r,{...e,children:(0,n.jsx)(d,{...e})}):d(e)}},3905:(e,r,s)=>{s.d(r,{ah:()=>c});var n=s(7294);function i(e,r,s){return r in e?Object.defineProperty(e,r,{value:s,enumerable:!0,configurable:!0,writable:!0}):e[r]=s,e}function t(e,r){var s=Object.keys(e);if(Object.getOwnPropertySymbols){var n=Object.getOwnPropertySymbols(e);r&&(n=n.filter((function(r){return Object.getOwnPropertyDescriptor(e,r).enumerable}))),s.push.apply(s,n)}return s}function a(e){for(var r=1;r<arguments.length;r++){var s=null!=arguments[r]?arguments[r]:{};r%2?t(Object(s),!0).forEach((function(r){i(e,r,s[r])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(s)):t(Object(s)).forEach((function(r){Object.defineProperty(e,r,Object.getOwnPropertyDescriptor(s,r))}))}return e}function o(e,r){if(null==e)return{};var s,n,i=function(e,r){if(null==e)return{};var s,n,i={},t=Object.keys(e);for(n=0;n<t.length;n++)s=t[n],r.indexOf(s)>=0||(i[s]=e[s]);return i}(e,r);if(Object.getOwnPropertySymbols){var t=Object.getOwnPropertySymbols(e);for(n=0;n<t.length;n++)s=t[n],r.indexOf(s)>=0||Object.prototype.propertyIsEnumerable.call(e,s)&&(i[s]=e[s])}return i}var l=n.createContext({}),c=function(e){var r=n.useContext(l),s=r;return e&&(s="function"==typeof e?e(r):a(a({},r),e)),s},d={inlineCode:"code",wrapper:function(e){var r=e.children;return n.createElement(n.Fragment,{},r)}},h=n.forwardRef((function(e,r){var s=e.components,i=e.mdxType,t=e.originalType,l=e.parentName,h=o(e,["components","mdxType","originalType","parentName"]),p=c(s),m=i,u=p["".concat(l,".").concat(m)]||p[m]||d[m]||t;return s?n.createElement(u,a(a({ref:r},h),{},{components:s})):n.createElement(u,a({ref:r},h))}));h.displayName="MDXCreateElement"}}]);