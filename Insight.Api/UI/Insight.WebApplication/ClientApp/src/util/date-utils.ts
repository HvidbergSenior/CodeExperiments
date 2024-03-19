export function getStartAndEndDateOfLastMonth(): {
  dateFrom: Date;
  dateTo: Date;
} {
  const currentDate = new Date();

  // Set the current date to the first day of the current month
  currentDate.setDate(1);

  // Subtract one day to get the last day of the previous month
  currentDate.setDate(0);

  // Clone the current date to get the last day of the previous month
  const endDate = new Date(currentDate);

  // Set the current date back to the first day of the current month
  currentDate.setDate(1);

  return {
    dateFrom: currentDate,
    dateTo: endDate,
  };
}

export function getDateAYearAgoFromStartOfMonth(): Date {
  const today = new Date();
  const firstOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);

  const firstOflastYear = new Date(
    firstOfMonth.getFullYear() - 1,
    firstOfMonth.getMonth(),
    firstOfMonth.getDate(),
  );

  return firstOflastYear;
}
