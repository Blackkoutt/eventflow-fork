import { DataTable } from "primereact/datatable";
import { User } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { useEffect } from "react";
import { Column } from "primereact/column";
import ActionsTemplate from "../../components/tabledata/ActionTemplate";
import HeaderTemplate from "../../components/tabledata/HeaderTemplate";
import { useTable } from "../../hooks/useTable";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import DetailsUserDialog from "../../components/management/users/DetailsUserDialog";

const UsersManagement = () => {
  const { data: items, get: getItems } = useApi<User>(ApiEndpoint.User);

  useEffect(() => {
    getItems({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    console.log(items);
  }, [items]);

  const cols = [
    {
      field: "id",
      header: "ID",
      sortable: true,
      body: (rowData: User) => rowData.id,
    },
    {
      field: "name",
      header: "Imię",
      sortable: true,
      body: (rowData: User) => rowData.name,
    },
    {
      field: "surname",
      header: "Nazwisko",
      sortable: true,
      body: (rowData: User) => rowData.surname,
    },
    {
      field: "emailAddress",
      header: "E-mail",
      sortable: true,
      body: (rowData: User) => rowData.emailAddress,
    },
    {
      field: "dateOfBirth",
      header: "Data urodzenia",
      sortable: true,
      body: (rowData: User) => DateFormatter.FormatDate(rowData.dateOfBirth, DateFormat.Date),
    },
    {
      field: "userRoles",
      header: "Role",
      sortable: false,
      body: (rowData: User) => rowData.userRoles.join(", "),
    },
    {
      header: "Akcja",
      sortable: false,
      body: (rowData: User) => (
        <ActionsTemplate
          onDetails={onDetails}
          includeDelete={false}
          includeModify={false}
          rowData={rowData}
        />
      ),
    },
  ];

  const {
    dt,
    filters,
    detailsDialog,
    itemToDetails,
    globalFilterValue,
    onGlobalFilterChange,
    menuElements,
    onDialogClose,
    onDetails,
  } = useTable<User[], User>(items, cols, "uzytkownicy");

  return (
    <div className="max-w-[64vw] self-center">
      <DetailsUserDialog
        maxWidth={1600}
        ref={detailsDialog}
        item={itemToDetails}
        onDialogClose={onDialogClose}
      />

      <DataTable
        ref={dt}
        value={items}
        paginator
        removableSort
        filters={filters}
        filterDisplay="row"
        globalFilterFields={[
          "name",
          "surname",
          "emailAddress",
          "dateOfBirth",
          "userRoles",
        ]}
        emptyMessage="Brak użytkowników"
        header={
          <HeaderTemplate
            headerText="Użytkownicy"
            // onCreate={onCreate}
            includeCreate={false}
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
export default UsersManagement;
