
<template>
  <div class="content-main">
    <div class="search-con search-con-top">
      <Form ref="formInline" label-position="right" :label-width="60" inline>
        <FormItem label="角色名">
          <Input class="search-input" v-model="queryData.SL_RoleName" />
        </FormItem>
          <Button class="search-btn" type="primary" @click="setPageData(1)">
            <Icon type="search" />&nbsp;&nbsp;搜索
          </Button>
        </FormItem>
      </Form>
    </div>

    <Card>
      <div>
        <Button class="search-btn" type="success" size="small" @click="handleAdd">
          <Icon type="md-add" />&nbsp;&nbsp;新增
        </Button>
      </div>
      <Table ref="tables" :data="tableData1" v-bind:columns="tableColumns1" stripe></Table>
      <!-- <tables ref="tables" editable v-model="tableData1" :columns="tableColumns1" @on-delete="handleDelete" stripe /> -->
      <div style="margin: 10px;overflow: hidden">
        <div style="float: right;">
          <Page
            :total="pageTotal"
            :current="pageCurrent"
            @on-change="changePage"
            @on-page-size-change="changePageSize"
            show-total
            show-elevator
            show-sizer
          ></Page>
        </div>
      </div>
    </Card>

    <!--垂直居中 class-name="vertical-center-modal" 和draggable冲突 draggable后mask强制变为false-->
    <Modal
      title="编辑"
      :mask-closable="false"
      v-model="modelEdit"
      width="800"
      scrollable
      footer-hide
      @on-ok="saveEdit"
    >
      <Edit ref="edit" :parent="this" :edit-row="eidtRow"></Edit>
    </Modal>

      <Modal
      title="授权"
      :mask-closable="false"
      v-model="modelPermission"
      width="300"
      scrollable
      footer-hide
    >
      <Permission ref="Permission" :parent="this" :edit-row="eidtRow"></Permission>
    </Modal>
  </div>
</template>
<script>
//import Tables from '_c/tables'
import "@/assets/css/util.less";
import Edit from "./Edit";
import Permission from "./Permission";
import { getPage, remove} from "@/api/pubRole";

export default {
  //  name: 'tables_page',
  components: {
    // Tables
    Edit,
    Permission
  },
  data() {
    return {
      tableData1: [],
      queryData: {},
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      modelPermission:false,
      isAdd: true,
      eidtRow: {},
      tableColumns1: [
        {
          title: "编号",
          key: "roleCode"
        },
        {
          title: "角色名",
          key: "roleName"
        },
        {
          title: "修改时间",
          key: "lmdt"
        },
        {
          title: "修改人",
          key: "lmid"
        },
        {
          title: "操作",
          key: "action",
          width: 300,
          align: "center",
          render: (h, params) => {
            return h("div", [
               h(
                "Button",
                {
                  props: {
                    type: "default",
                    size: "small",
                    icon: "md-key"
                  },
                  style: {
                    marginRight: "5px"
                  },
                  on: {
                    click: () => {
                      this.handlePermission(params);
                    }
                  }
                },
                "授权"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "primary",
                    size: "small",
                    icon: "md-create"
                  },
                  style: {
                    marginRight: "5px"
                  },
                  on: {
                    click: () => {
                      this.handleEdit(params);
                    }
                  }
                },
                "编辑"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "error",
                    size: "small",
                    icon: "md-trash"
                  },
                  on: {
                    click: () => {
                      this.handleDelete(params);
                    }
                  }
                },
                "删除"
              )
            ]);
          }
        }
        // {
        //     title: 'Updated Time',
        //     key: 'update',
        //     render: (h, params) => {
        //         return h('div', this.formatDate(this.tableData1[params.index].update));
        //     }
        // }
      ]
    };
  },
  computed: {},
  methods: {
    setPageData(pageNum = this.pageCurrent, pageSize = 10) {
      getPage({
        pageNum: pageNum,
        pageSize: pageSize,
        field: "Id",
        order: "asc",
        query: this.queryData
      })
        .then(res => {
          const resData = res.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code != 1) {
            this.$Message.error(code.msg);
            return;
          }
          const data = resData.data;
          this.tableData1 = data;
          this.pageTotal = resData.count;
          this.pageCurrent = resData.PageNum;
        })
        .catch(err => {});
    },
    formatDate(date) {
      const y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      return y + "-" + m + "-" + d;
    },
    changePage(page) {
      this.setPageData(page);
    },
    changePageSize(pageSize) {
      this.setPageData(1, pageSize);
    },
    handleDelete(params) {
      var row = params.row;
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + row.id + "]?</p>",
        onOk: () => {
          // this.$Message.info("Clicked ok");
          this.remove(row);
        },
        onCancel: row => {}
      });
    },
    handleAdd() {
      this.$refs.edit.handleReset();
      this.modelEdit = true;
      this.isAdd = true;
      this.eidtRow = {};
    },
    handleEdit(params) {
      this.modelEdit = true;
      this.isAdd = false;
      this.eidtRow = params.row;
    },
    handlePermission(params) {
      this.modelPermission = true;
      this.eidtRow = params.row;
    },
    remove(row) {
      var id = row.id;
      remove(id)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("删除成功");
            this.setPageData();
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => {});
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    }
  },
  mounted() {
    this.setPageData(1);
  }
};
</script>

