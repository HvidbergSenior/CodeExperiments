import { TextField } from "@mui/material";
import { useCallback, useRef } from "react";

type Props = {
  label: string;
  startValue: string;
  onUpdate: (value: string) => void;
};
export const DebouncedStandardTextfield = ({
  label,
  startValue,
  onUpdate: updateFilter,
}: Props) => {
  const debounceTimer = useRef<NodeJS.Timeout>();

  const handleOnChange = useCallback(
    (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      if (debounceTimer.current) {
        clearTimeout(debounceTimer.current);
      }

      debounceTimer.current = setTimeout(() => {
        updateFilter(event.target.value);
      }, 500);
    },
    [updateFilter],
  );

  return (
    <TextField
      size="small"
      label={label}
      defaultValue={startValue}
      sx={{ ml: 2 }}
      inputProps={{ style: { fontSize: 14 } }}
      InputLabelProps={{ style: { fontSize: 14 } }}
      variant="standard"
      onChange={handleOnChange}
    />
  );
};
