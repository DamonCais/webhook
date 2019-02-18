import html2canvas from "html2canvas";

export async function html2Png(el, isPrint = true, fileName = 'html.png') {
  if (!el) {
    return;
  }
  let printDom = el;
  let width = printDom.offsetWidth;
  let height = printDom.offsetHeight;
  const canvas2 = document.createElement("canvas");
  const scale = 2;
  canvas2.width = width * scale;
  canvas2.height = height * scale;
  const context1 = canvas2.getContext("2d");
  if (context1) {
    context1.scale(scale, scale);
  }
  const opts = {
    scale,
    canvas: canvas2,
    // logging: true, //日志开关，便于查看html2canvas的内部执行流程
    width,
    height,
    // 【重要】开启跨域配置
    useCORS: true
  };
  let canvas = await html2canvas(printDom, opts);
  const context = canvas2.getContext("2d");
  if (context) {
    context.scale(2, 2);
    context.mozImageSmoothingEnabled = false;
    context.webkitImageSmoothingEnabled = false;
    context.imageSmoothingEnabled = false;
  }
  // canvas转换成url，然后利用a标签的download属性，直接下载，绕过上传服务器再下载
  var dataURL = canvas.toDataURL("image/png");
  if (isPrint) {
    var iframe = document.createElement("IFRAME");
    var doc = null;
    document.body.appendChild(iframe);
    doc = iframe.contentWindow.document;
    let str = `<div>
      <img style="width:100%;" src="${dataURL}" />
    </div>`;
    doc.write(str);
    doc.close();
    iframe.contentWindow.focus();
    setTimeout(() => {
      iframe.contentWindow.print();
      document.body.removeChild(iframe);
    }, 2000);
  } else {
    return getBlob(canvas);
  }
}

function getBlob(canvas) {
  return new Promise((resolve, reject) => {
    canvas.toBlob((blob) => {
      resolve(blob);
      // const link = document.createElement("a");
      // link.href = URL.createObjectURL(blob);
      // link.setAttribute("download", fileName);
      // document.body.appendChild(link);
      // link.click();
      // document.body.removeChild(link);
    });
  })
}