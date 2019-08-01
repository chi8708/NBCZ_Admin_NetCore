
<template>
  <div >
    <div class="search-con search-con-top">
      用户名：
      <Input placeholder="default size" class="search-input" />
      <Button class="search-btn" type="primary">
        <Icon type="search" />&nbsp;&nbsp;搜索
      </Button>
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
    <Modal title="编辑"  :mask-closable="false"   v-model="modelEdit" width="800"  scrollable footer-hide
    @on-ok="SaveEdit"
    >
      <Edit ref="edit" :parent='this' :edit-row="eidtRow" ></Edit>
    </Modal>
  </div>
</template>
<script>
//import Tables from '_c/tables'
import "@/assets/css/util.less";
import "@/components/tables/index.less";
import Edit from "./Edit";
import { getPage } from "@/api/pubUser";

export default {
  //  name: 'tables_page',
  components: {
    // Tables
    Edit
  },
  data() {
    return {
      tableData1: [],
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      isAdd:true,
      eidtRow: {},
      tableColumns1: [
        {
          title: "编号",
          key: "id"
        },
        {
          title: "用户名",
          key: "userName"
        },
        {
          title: "真实姓名",
          key: "realName"
        },
        {
          title: "电话",
          key: "tel"
        },
        {
          title: "部门",
          key: "deptName"
        },
        {
          title: "修改时间",
          key: "lmdt"
        },
        {
          title: "操作",
          key: "action",
          width: 200,
          align: "center",
          render: (h, params) => {
            return h("div", [
              // ios-create-outline
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
    setPageData(pageNum, pageSize = 10) {
      getPage({
        pageNum: pageNum,
        pageSize: pageSize,
        field: "Id",
        order: "asc"
      })
        .then(res => {
          const resData = res.data;
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
          this.$Message.info("Clicked ok");
        },
        onCancel: () => {}
      });
    },
    handleAdd() {
      this.$refs.edit.handleReset();
      this.modelEdit = true;
      this.isAdd=true;
       this.eidtRow={};
    },
    handleEdit(params) {
      this.modelEdit = true;
      this.isAdd=false;
      this.eidtRow = params.row;
    },
    SaveEdit(){
      var row= this.$refs.edit.Row;
    }
  },
  mounted() {
    this.setPageData(1);
  }
};
</script>

