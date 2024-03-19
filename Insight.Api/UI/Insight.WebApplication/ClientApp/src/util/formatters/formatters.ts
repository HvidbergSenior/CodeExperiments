export const formatNumber = (
  largeNumber: number,
  maximumFractionDigits = 2,
) => {
  const result = new Intl.NumberFormat("da-DK", {
    maximumFractionDigits,
  }).format(largeNumber);
  return result;
};

export const formatNumberFromString = (
  largeNumber: string,
  maximumFractionDigits = 2,
) => {
  const formattedNumber: string = new Intl.NumberFormat("en-US", {
    maximumFractionDigits,
  }).format(parseFloat(largeNumber));

  return formattedNumber;
};

export const formatPercentage = (
  decimalNumber: number,
  maximumFractionDigits = 2,
) => {
  const result = new Intl.NumberFormat("da-DK", {
    maximumFractionDigits,
  }).format(decimalNumber * 100);
  return result + "%";
};

export const formatDate = (date: Date) => {
  const day = date.getDate().toString().padStart(2, "0");
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const year = date.getFullYear();
  const formattedDate = `${year}-${month}-${day}`;
  return formattedDate;
};

type DateFormat = "dd-mm-yyyy" | "dd/mm-yyyy";

export const formatDateOnly = (dateString: string, format?: DateFormat) => {
  if (!format) return dateString;

  const [year, month, day] = dateString.split("-").map(String);
  if (format === "dd-mm-yyyy") {
    return `${day}-${month}-${year}`;
  } else {
    return `${day}/${month}-${year}`;
  }
};

const padNumber = (value?: number, size: number = 2) => {
  if (!value) return "00";

  let s = value.toString();
  while (s.length < size) {
    s = `0${s}`;
  }

  return s;
};

const convertMillisecondsToHMS = (milliseconds: number) => {
  const hours = Math.floor(milliseconds / 3600000);
  const minutes = Math.floor((milliseconds % 3600000) / 60000);
  const seconds = Math.floor((milliseconds % 60000) / 1000);

  return {
    hours: hours,
    minutes: minutes,
    seconds: seconds,
  };
};

export const getHMSFromMilliseconds = (milliseconds?: number) => {
  if (!milliseconds || milliseconds === 0) {
    return "00:00:00";
  }
  const { hours, minutes, seconds } = convertMillisecondsToHMS(milliseconds);

  return `${padNumber(hours, 2)}:${padNumber(minutes, 2)}:${padNumber(
    seconds,
    2,
  )}`;
};

export const setAsteriskCardNumber = (lastDigits: string) => {
  return `******${lastDigits}`;
};

export function capitalizeFirstLetter(word: string) {
  return word.charAt(0).toUpperCase() + word.slice(1);
}
