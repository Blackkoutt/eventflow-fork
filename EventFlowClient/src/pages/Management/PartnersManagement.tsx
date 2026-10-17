import { DataTable } from "primereact/datatable";
import { Partner } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import CreatePartnerDialog from "../../components/management/partners/CreatePartnerDialog";
import DeletePartnerDialog from "../../components/management/partners/DeletePartnerDialog";
import ModifyPartnerDialog from "../../components/management/partners/ModifyPartnerDialog";
import ApiClient from "../../services/api/ApiClientService";

const PartnersManagement = () => {
  const { data: items, get: getItems } = useApi<Partner>(ApiEndpoint.Partner);

  useEffect(() => {
    getItems({ id: undefined, queryParams: { kind: "Partner" } });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: Partner) => rowData.id,
    },
    {
      field: "name",
      header: "Nazwa",
      sortable: true,
      body: (rowData: Partner) => rowData.name,
    },
    {
      field: "photoEndpoint",
      header: "Zdjęcie",
      body: (rowData: Partner) => (
        <img
          src={`${ApiClient.GetPhotoEndpoint(rowData.photoEndpoint)}`}
          alt={`Zdjęcie partnera o id ${rowData.id}`}
          className="w-[100px] h-[100px] object-contain"
        />
      ),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: Partner) => (
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
  } = useTable<Partner[], Partner>(items, cols, "partnerzy");

  const reloadItemsAfterSuccessDialog = () => {
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: { kind: "Partner" } });
  };

  return (
    <div className="max-w-[64vw] self-center">
      <CreatePartnerDialog
        minWidth={300}
        maxWidth={600}
        ref={createDialog}
        onDialogClose={onDialogClose}
        paddingX={32}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <DeletePartnerDialog
        ref={deleteDialog}
        maxWidth={750}
        item={itemToDelete}
        onDialogClose={onDialogClose}
        onDialogSuccess={reloadItemsAfterSuccessDialog}
      />

      <ModifyPartnerDialog
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
        emptyMessage="Brak partnerów"
        header={
          <HeaderTemplate
            headerText="Partnerzy"
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
export default PartnersManagement;
