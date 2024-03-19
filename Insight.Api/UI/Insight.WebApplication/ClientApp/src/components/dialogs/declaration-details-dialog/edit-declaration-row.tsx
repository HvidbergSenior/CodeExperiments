import { Box, TextField, Typography } from "@mui/material";
import { Controller } from "react-hook-form";
import { EditDeclarationData } from "../../../pages/types";

interface Props {
  data: EditDeclarationData;
  editActive: boolean;
}
export const EditDeclarationRow = ({ data, editActive }: Props) => {
  return (
    <Box
      key={data.name}
      display="flex"
      flexDirection="row"
      justifyContent="space-between"
      alignItems="center"
      mt={2}
      sx={{
        backgroundColor: (theme) => theme.palette.Gray1.main,
        height: "70px",
        borderRadius: "10px",
      }}
    >
      <Box pl={5} width="50%">
        <Typography variant="h5">
          {data.name[0].toUpperCase() + data.name.slice(1)}
        </Typography>
      </Box>
      <Box width="50%" pr={2}>
        {editActive ? (
          <Controller
            name={data.name}
            control={data.control}
            render={({ field }) => <TextField {...field} variant="outlined" />}
          />
        ) : (
          <Typography variant="h5" sx={{ fontWeight: "500" }}>
            {typeof data.value === "boolean"
              ? data.value.toString()
              : data.value}
          </Typography>
        )}
      </Box>
    </Box>
  );
};
