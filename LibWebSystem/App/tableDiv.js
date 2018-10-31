'use strict';

var app = angular.module("LibWebSystem");

app.provider("tabelaPadrao", function () {

    this.$get = function () {

        var Tabela = {
            gerar: function (object) {
                object.table.bootstrapTable('destroy');
                object.table.bootstrapTable({
                    search: ((object.search !== undefined) ? object.search : true),
                    showExport: ((object.showExport !== undefined) ? object.showExport : true),
                    reorderableRows: true,
                    reorderableColumns: true,
                    detailView: object.detailView,
                    minimumCountColumns: 2,
                    showPaginationSwitch: false,
                    showPrint: ((object.showPrint !== undefined) ? object.showPrint : true),
                    pagination: ((object.pagination !== undefined) ? object.pagination : true),
                    pageSize: 10,
                    pageList: [10, 20, 50, 100, 200, 500, 1000, 2000, object.data.length].filter((v, i, a) => a.indexOf(v) === i && v <= object.data.length),
                    showFooter: false,
                    striped: false,
                    clickToSelect: false,
                    maintainSelected: false,
                    cache: false,
                    exportDataType: 'all',
                    exportTypes: ['xlsx', 'csv', 'txt', 'json', 'pdf'],
                    onExpandRow: object.detailsRows,
                    columns: object.columns,
                    printPageBuilder: object.pageBuilder,
                    onCheck: object.onCheck,
                    onUncheck: object.onUncheck,
                    onCheckAll: object.onCheckAll,
                    onUncheckAll: object.onUncheckAll,
                });
                object.table.bootstrapTable('load', object.data);
            }
        }

        return Tabela;
    }
});
