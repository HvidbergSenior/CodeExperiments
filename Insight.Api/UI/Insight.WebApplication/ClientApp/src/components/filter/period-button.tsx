import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import {
  Box,
  BoxProps,
  Button,
  Popover,
  Stack,
  Typography,
} from "@mui/material";
import { useCallback, useEffect, useRef, useState } from "react";
import { theme } from "../../theme/theme";
import { DateCalendar } from "@mui/x-date-pickers";
import { errorTranslations } from "../../translations/errors";
import { commonTranslations } from "../../translations/common";
import { filterTranslations } from "../../translations/filter";

interface Props extends BoxProps {
  dateFrom: string | undefined;
  dateTo: string | undefined;
  updateFilter: (fromDate: Date, toDate: Date) => void;
}
export const PeriodButton = ({
  dateFrom,
  dateTo,
  updateFilter,
  ...props
}: Props) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const anchorRef = useRef<HTMLButtonElement>(null);
  const startDateDefault =
    dateFrom === undefined
      ? new Date(new Date().setMonth(new Date().getMonth() - 1))
      : new Date(dateFrom);
  const endDateDefault = dateTo === undefined ? new Date() : new Date(dateTo);

  const [startDate, setStartDate] = useState<Date | null>(startDateDefault);
  const [endDate, setEndDate] = useState<Date | null>(endDateDefault);
  const [dateError, setDateError] = useState<string | boolean>(false);

  const options: Intl.DateTimeFormatOptions = {
    weekday: "long",
    year: "numeric",
    month: "short",
    day: "numeric",
  };

  useEffect(() => {
    setStartDate(new Date(dateFrom ?? startDateDefault));
  }, [dateFrom]);

  useEffect(() => {
    setEndDate(new Date(dateTo ?? endDateDefault));
  }, [dateFrom]);

  const handleDateChange = useCallback(() => {
    if (!startDate || !endDate) {
      setDateError(errorTranslations.errorDateRangePickerNullDate);
      return;
    }
    if (startDate > endDate) {
      setDateError(
        errorTranslations.errorDateRangePickerFromDateIsLaterThanStartDate,
      );
      return;
    }
    updateFilter(startDate, endDate);
    setIsOpen(false);
  }, [startDate, endDate, updateFilter]);

  return (
    <>
      <Button
        ref={anchorRef}
        onClick={() => setIsOpen(!isOpen)}
        sx={{
          borderRadius: "5px",
          border: isOpen ? "1px solid black" : `1px solid #c4c4c4`,
          width: { xs: "100%", sm: "240px" },
          height: { xs: "37px", sm: "auto" },
          minWidth: "130px",
          justifyContent: "space-between",
          "&:hover": {
            border: "1px solid black",
            backgroundColor: theme.palette.common.white,
          },
          "&:focus": {
            backgroundColor: theme.palette.common.white,
            color: "black",
          },
          ...props,
        }}
      >
        <Typography
          sx={{ fontSize: "14px" }}
          color="grey"
          fontWeight={500}
          fontSize={16}
        >
          {dateFrom === undefined
            ? filterTranslations.period
            : dateFrom + " - " + dateTo}
        </Typography>
        <ArrowDropDownIcon />
      </Button>
      <Popover
        open={isOpen}
        anchorEl={anchorRef.current}
        onClose={() => setIsOpen(!isOpen)}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left",
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "left",
        }}
      >
        <Box
          sx={{
            width: { xs: "auto", sm: "800px" },
            height: { xs: "auto", sm: "450px" },
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
            m: theme.spacing(5),
          }}
        >
          <Box
            mb={2}
            sx={{
              display: "flex",
              justifyContent: "space-evenly",
              flexDirection: { xs: "column", sm: "row" },
            }}
          >
            <Box sx={{ display: "flex", flexDirection: "column" }}>
              <Typography mb={2} mt={2} variant="h5">
                {filterTranslations.from}{" "}
                {dateFrom === undefined
                  ? filterTranslations.notSelected
                  : startDate?.toLocaleDateString("en-UK", options)}
              </Typography>
              <DateCalendar
                sx={{
                  mt: theme.spacing(2),

                  background: theme.palette.common.white,
                  borderRadius: "30px",
                }}
                value={startDate}
                onChange={(newValue) => {
                  setDateError(false);
                  setStartDate(newValue);
                }}
              />
            </Box>
            <Box sx={{ display: "flex", flexDirection: "column" }}>
              <Typography mb={2} mt={2} variant="h5">
                {filterTranslations.to}{" "}
                {dateTo === undefined
                  ? filterTranslations.notSelected
                  : endDate?.toLocaleDateString("en-UK", options)}
              </Typography>
              <DateCalendar
                sx={{
                  mt: theme.spacing(2),
                  background: theme.palette.common.white,
                  borderRadius: "30px",
                }}
                value={endDate}
                onChange={(newValue) => {
                  setDateError(false);
                  setEndDate(newValue);
                }}
              />
            </Box>
          </Box>

          <Stack
            gap="16px"
            sx={{
              display: "flex",
              flexDirection: "row",
              justifyContent: "end",
              alignItems: "center",
              m: theme.spacing(8),
            }}
          >
            {dateError && (
              <Typography mr="10px" variant="error">
                {dateError}
              </Typography>
            )}
            <Button sx={{ width: "140px" }} onClick={() => setIsOpen(false)}>
              {commonTranslations.cancel}
            </Button>
            <Button sx={{ width: "140px" }} onClick={() => handleDateChange()}>
              {filterTranslations.apply}
            </Button>
          </Stack>
        </Box>
      </Popover>
    </>
  );
};
