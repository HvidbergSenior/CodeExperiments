import { Box, Typography } from "@mui/material";
import { DeclarationDetailRowInput } from "./published-details-dialog";

interface Props {
  data: DeclarationDetailRowInput;
}
export const DeclarationDetailRow = ({ data }: Props) => {
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
        <Typography variant="h5" sx={{ fontWeight: "500" }}>
          {typeof data.value === "boolean" ? data.value.toString() : data.value}
        </Typography>
      </Box>
    </Box>
  );
};
