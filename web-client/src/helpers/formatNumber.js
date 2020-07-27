function formatNumber(number, minPrecision = 0, maxPrecision = 6) {
  if (number) {
    return number.toLocaleString("pt-BR", {
      minimumFractionDigits: minPrecision,
      maximumFractionDigits: maxPrecision,
    });
  }

  return null;
}

export default formatNumber;
