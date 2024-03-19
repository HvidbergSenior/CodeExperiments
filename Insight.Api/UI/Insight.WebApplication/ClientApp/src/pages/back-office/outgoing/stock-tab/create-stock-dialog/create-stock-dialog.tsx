import {
  FormControl,
  FormHelperText,
  MenuItem,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { Box } from "@mui/system";
import { DatePicker } from "@mui/x-date-pickers";
import { ChangeEvent } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { OutgoingFuelTransactionResponse } from "../../../../../api/api";
import { OperationDialog } from "../../../../../components/dialogs/operation-dialog";
import { DialogBaseProps } from "../../../../../shared/types";
import { commonTranslations } from "../../../../../translations/common";
import { stockTabTranslations } from "../../../../../translations/pages/stock-tab-translations";
import { formatDate } from "../../../../../util/formatters/formatters";
import { useCreateStock } from "./use-create-stock";

interface Props extends DialogBaseProps {
  fuelTransaction?: OutgoingFuelTransactionResponse;
  onSubmit: () => Promise<void>;
}

interface StockFormData {
  storage: string;
  country: string;
  product: string;
  company: string;
  volume: string;
  period: Date | undefined;
}

export const CreateStockDialog = ({
  fuelTransaction,
  onSubmit,
  ...props
}: Props) => {
  const useCreateStockProps = useCreateStock(fuelTransaction);

  const { formState, handleSubmit, control, resetField, getValues } =
    useForm<StockFormData>({
      defaultValues: {
        storage: fuelTransaction?.location ?? "",
        country: fuelTransaction?.country ?? "",
        product: fuelTransaction?.productName ?? "",
        company: "",
        volume: undefined,
        period: undefined,
      },
    });

  const handleCreateStock: SubmitHandler<StockFormData> = async (data) => {
    const result = await useCreateStockProps.createStock({
      storage: data.storage,
      country: data.country,
      product: data.product,
      companyId: data.company,
      volume: parseFloat(data.volume),
      period: data.period === undefined ? "" : formatDate(data.period),
    });
    if (result) {
      onSubmit();
    }
  };

  const handleEnterKeyDown = (event: React.KeyboardEvent<HTMLDivElement>) => {
    if (event.key === "Enter") {
      handleSubmit(handleCreateStock)();
    }
  };

  const handleCompanyChange = (
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    const selectedProduct = getValues("product");
    const productStillValid = useCreateStockProps.onCompanyChanged(
      event.target.value,
      selectedProduct,
    );
    if (!productStillValid) {
      resetField("product");
    }
  };

  return (
    <OperationDialog
      title={stockTabTranslations.createStockDialog.title}
      isOpen={props.isOpen}
      onConfirm={handleSubmit(handleCreateStock)}
      onClose={props.onClose}
      submitTitle={stockTabTranslations.createStockDialog.submitButton}
      cancelTitle={commonTranslations.cancel}
      disableSubmit={useCreateStockProps.disableSubmit}
      isLoading={useCreateStockProps.isLoading}
      maxWidth={"md"}
      scroll="body"
    >
      <Box sx={{ m: 2 }} />
      <Stack direction="row" gap={30}>
        <Stack direction="column" width="100%" gap={4}>
          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="company"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.company,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={stockTabTranslations.createStockDialog.company}
                  variant="outlined"
                  onChange={(value) => {
                    field.onChange(value); //Important to not override the react-hook-form functionality that sets the selected value
                    handleCompanyChange(value);
                  }}
                >
                  {useCreateStockProps.companies.map((option) => (
                    <MenuItem key={option.id} value={option.id}>
                      <Box height="24px" display="flex" alignItems="center">
                        <Typography variant="body1" height="16px">
                          {option.name}
                        </Typography>
                      </Box>
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />

            <FormHelperText
              error={!!formState.errors.company}
              role={formState.errors.company ? "alert" : undefined}
            >
              {formState.errors.company?.message ?? ""}
            </FormHelperText>
          </FormControl>

          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="storage"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.storage,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={stockTabTranslations.createStockDialog.storage}
                  variant="outlined"
                  onKeyDown={handleEnterKeyDown}
                />
              )}
            />

            <FormHelperText
              error={!!formState.errors.storage}
              role={formState.errors.storage ? "alert" : undefined}
            >
              {formState.errors.storage?.message ?? ""}
            </FormHelperText>
          </FormControl>

          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="country"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.country,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={stockTabTranslations.createStockDialog.country}
                  variant="outlined"
                  onKeyDown={handleEnterKeyDown}
                />
              )}
            />

            <FormHelperText
              error={!!formState.errors.country}
              role={formState.errors.country ? "alert" : undefined}
            >
              {formState.errors.country?.message ?? ""}
            </FormHelperText>
          </FormControl>
        </Stack>

        <Stack direction="column" width="100%" gap={4}>
          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="product"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.product,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={stockTabTranslations.createStockDialog.product}
                  variant="outlined"
                  disabled={!useCreateStockProps.isProductSelectionEnabled}
                >
                  {useCreateStockProps.selectableProducts.map((option) => (
                    <MenuItem key={option.number} value={option.number}>
                      <Box height="24px" display="flex" alignItems="center">
                        <Typography variant="body1" height="16px">
                          {option.name} ({option.number})
                        </Typography>
                      </Box>
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />

            <FormHelperText
              error={!!formState.errors.product}
              role={formState.errors.product ? "alert" : undefined}
            >
              {formState.errors.product?.message ?? ""}
            </FormHelperText>
          </FormControl>
          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="volume"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.volume,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{
                    style: { fontSize: 14 },
                    type: "number", // Only allowing number input, but also showing up/down and allowing scroll/key up/down
                    pattern: "[0-9]*", // Not working
                  }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={stockTabTranslations.createStockDialog.volume}
                  variant="outlined"
                  // Disable showing up/down buttons
                  sx={{
                    "& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button":
                      {
                        display: "none",
                      },
                    "& input[type=number]": {
                      MozAppearance: "textfield",
                    },
                  }}
                  // Disable scroll up/down
                  onFocus={(e) =>
                    e.target.addEventListener(
                      "wheel",
                      function (e) {
                        e.preventDefault();
                      },
                      { passive: false },
                    )
                  }
                  // Disable negative numbers
                  onKeyPress={(event) => {
                    if (event?.key === "-" || event?.key === "+") {
                      event.preventDefault();
                    }
                  }}
                  // Disable key up/down
                  onKeyDown={(event) => {
                    if (
                      event?.code === "ArrowDown" ||
                      event?.code === "ArrowUp"
                    ) {
                      event.preventDefault();
                    }
                    handleEnterKeyDown(event);
                  }}
                  onWheel={(event) => {
                    event.currentTarget.blur();
                  }}
                />
              )}
            />

            <FormHelperText
              error={!!formState.errors.volume}
              role={formState.errors.volume ? "alert" : undefined}
            >
              {formState.errors.volume?.message ?? ""}
            </FormHelperText>
          </FormControl>
          <FormControl sx={{ height: "60px" }}>
            <Controller
              name="period"
              control={control}
              rules={{
                required: stockTabTranslations.createStockDialog.required(
                  stockTabTranslations.createStockDialog.period,
                ),
              }}
              render={({ field }) => (
                <DatePicker
                  {...field}
                  label={stockTabTranslations.createStockDialog.period}
                  slotProps={{
                    textField: { size: "small", error: false },
                  }}
                />
              )}
            />

            <FormHelperText
              error={!!formState.errors.period}
              role={formState.errors.period ? "alert" : undefined}
            >
              {formState.errors.period?.message ?? ""}
            </FormHelperText>
          </FormControl>
        </Stack>
      </Stack>
      <Box
        display="flex"
        width="100%"
        flexDirection="row"
        justifyContent="end"
        alignItems="center"
        mt={4}
      >
        {useCreateStockProps.apiError && (
          <Typography variant="error">
            {useCreateStockProps.apiError.detail}
          </Typography>
        )}
      </Box>
    </OperationDialog>
  );
};
