function formatNumber(number, minPrecision = 0, maxPrecision = 6) {
  if (number) {
    return number.toLocaleString("en-US", {
      minimumFractionDigits: minPrecision,
      maximumFractionDigits: maxPrecision,
    });
  }

  return null;
}

export default formatNumber;
