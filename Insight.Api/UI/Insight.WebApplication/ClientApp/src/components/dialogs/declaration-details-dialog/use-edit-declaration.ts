import React from "react";
import { authorizedHttpClient } from "../../../api";
import { IncomingDeclarationDto } from "../../../api/api";
import useHandleNetworkError from "../../../util/errors/use-handle-network-error";

interface Props {
  declarationId: string;
}
export const useEditDeclaration = ({ declarationId }: Props) => {
  const [loading, setLoading] = React.useState(true);
  const [declaration, setDeclaration] = React.useState<
    IncomingDeclarationDto | undefined
  >(undefined);
  const { showErrorDialog } = useHandleNetworkError();

  const getDeclaration = async () => {
    try {
      const response =
        await authorizedHttpClient.api.getIncomingDeclarationById(
          declarationId,
        );

      setDeclaration(response.data);
    } catch (error) {
      showErrorDialog(error);
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    getDeclaration();
  }, []);

  return { declaration, loading };
};
