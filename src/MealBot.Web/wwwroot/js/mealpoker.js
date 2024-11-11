function copyTextInElement(elementId) {
  // Get the text field
  var copyText = document.getElementById(elementId).innerText;

   // Copy the text inside the text field
  navigator.clipboard.writeText(copyText);
}