import { TextField } from "@mui/material";
import { useEffect, useState } from "react";
import SearchIcon from "@mui/icons-material/Search";

type Props = {
  label: string;
  startValue: string;
  updateFilter: (filterValue: string) => void;
};
export const DebouncedTextfield = ({
  label,
  startValue,
  updateFilter,
}: Props) => {
  const [debouncedValue, setDebouncedValue] = useState<string>(startValue);

  useEffect(() => {
    setDebouncedValue(startValue);
  }, [startValue]);

  useEffect(() => {
    const timer = setTimeout(() => {
      updateFilter(debouncedValue);
    }, 500);

    return () => {
      clearTimeout(timer);
    };
  }, [debouncedValue]);

  return (
    <TextField
      size="small"
      label={label}
      value={debouncedValue}
      inputProps={{ style: { fontSize: 14 } }}
      InputLabelProps={{ style: { fontSize: 14 } }}
      sx={{ width: "auto", maxWidth: "500px" }}
      onChange={(event) => setDebouncedValue(event.target.value)}
      InputProps={{ endAdornment: <SearchIcon /> }}
    />
  );
};
