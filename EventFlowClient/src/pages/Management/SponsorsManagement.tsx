import { DataTable } from "primereact/datatable";
import { Sponsor } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import CreateSponsorDialog from "../../components/management/sponsors/CreateSponsorDialog";
import DeleteSponsorDialog from "../../components/management/sponsors/DeleteSponsorDialog";
import ModifySponsorDialog from "../../components/management/sponsors/ModifySponsorDialog";
import ApiClient from "../../services/api/ApiClientService";

const SponsorsManagement = () => {
  const { data: items, get: getItems } = useApi<Sponsor>(ApiEndpoint.Sponsor);

  useEffect(() => {
    getItems({ id: undefined, queryParams: { kind: "Sponsor" } });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: Sponsor) => rowData.id,
    },
    {
      field: "name",
      header: "Nazwa",
      sortable: true,
      body: (rowData: Sponsor) => rowData.name,
    },
    {
      field: "photoEndpoint",
      header: "Zdjęcie",
      body: (rowData: Sponsor) => (
        <img
          src={`${ApiClient.GetPhotoEndpoint(rowData.photoEndpoint)}`}
          alt={`Zdjęcie sponsora o id ${rowData.id}`}
          className="w-[80px] h-[80px] object-contain"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: Sponsor) => (
        <ActionsTemplate
          rowData={rowData}
          includeDetails={false}
          onModify={onModify}
          onDelete={onDelete}
        />
      ),
    },
  ];

  const {
    dt,
    deleteDialog,
    createDialog,
    modifyDialog,
    itemToDelete,
    itemToModify,
    filters,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDelete,
    onModify,
    onCreate,
    closeDialogsAndSetValuesToDefault,
  } = useTable<Sponsor[], Sponsor>(items, cols, "sponsorzy");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: { kind: "Sponsor" } });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreateSponsorDialog
        minWidth={300}
        maxWidth={600}
        ref={createDialog}
        onDialogClose={onDialogClose}
        paddingX={32}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DeleteSponsorDialog
        ref={deleteDialog}
        maxWidth={750}
        item={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <ModifySponsorDialog
        ref={modifyDialog}
        item={itemToModify}
        minWidth={400}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DataTable
        ref={dt}
        value={items}
        paginator
        removableSort
        filters={filters}
        filterDisplay="row"
        globalFilterFields={["name"]}
        emptyMessage="Brak sponsorów"
        header={
          <HeaderTemplate
            headerText="Sponsorzy"
            onCreate={onCreate}
            menuElements={menuElements}
            globalFilterValue={globalFilterValue}
            onGlobalFilterChange={(e) => onGlobalFilterChange(e)}
          />
        }
        rows={5}
        style={{ minWidth: "62vw" }}
        rowsPerPageOptions={[5, 10, 25, 50]}
        stripedRows
        showGridlines
      >
        {cols.map((col, index) => (
          <Column
            key={index}
            field={col.field}
            header={col.header}
            body={(rowData) => col.body(rowData)}
            sortable={col.sortable}
          />
        ))}
      </DataTable>
    </div>
  );
};
export default SponsorsManagement;
