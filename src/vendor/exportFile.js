import JSZip from "jszip";
import FileSaver from "file-saver";


export async function export2Zip(arr) {
  const zip = new JSZip();

  for (var i = 0; i < arr.length; i++) {
    await zip.file(arr[i].fname, arr[i].data, {
      binary: true
    });
  }
  let content = await zip.generateAsync({
    type: "blob"
  });
  FileSaver.saveAs(content, "打包.zip");
}

export async function export2Single(arr) {
  for (var i = 0; i < arr.length; i++) {
    const url = window.URL.createObjectURL(new Blob([arr[i].data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", arr[i].fname);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}

export function exportFile(obj) {
  let arr = [];
  if (Object.prototype.toString.call(obj) === "[object Array]") {
    arr = obj;
  } else {
    arr.push(obj);
  }
  if (arr.length < 5) {
    return export2Single(arr);
  } else {
    return export2Zip(arr);
  }
}
