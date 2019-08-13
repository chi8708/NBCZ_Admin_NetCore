<style>
.content-main{
  height: 99%;
  border: 1px solid #dcdee2;
  /* background-color: #fff; */
}
.demo-split-pane {
  padding: 10px;
}
</style>
<template>
  <div class="content-main">
    <Split v-model="split1" max="300" min="300">
      <div slot="left" class="demo-split-pane">
        <h3>权限</h3>
        <function-tree ref="functionTree" :parent="this"></function-tree>
      </div>
      <div slot="right" class="demo-split-pane">
        <h3>同级及下级</h3>
        <div>
          <Button class="search-btn" type="success" size="small" @click="handleAdd">
            <Icon type="md-add" />&nbsp;&nbsp;新增
          </Button>
        </div>
        <div>
            <Table ref="tables" height="700"  :data="tableData1" v-bind:columns="tableColumns1" stripe></Table>
        </div>
      
      </div>
    </Split>
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
  </div>
</template>
<script>
import FunctionTree from "./FunctionTree"
import Edit from "./Edit"
import {getChildList,remove} from "@/api/pubFunction"
export default {
  data() {
    return {
      split1: "300",
      tableData1: [],
      pageTotal: 0,
      pageCurrent: 1,
      modelEdit: false,
      isAdd: true,
      eidtRow: {},
      selectedCode:"",
      tableColumns1: [
        {
          title: "编号",
          key: "functionCode"
        },
        {
          title: "中文名",
          key: "functionChina"
        },
        {
          title: "英文名",
          key: "functionEnglish"
        },
        {
          title: "修改时间",
          key: "editdate"
        },
         {
          title: "修改人",
          key: "editor"
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
      ]
    };
  },
  components: {
    FunctionTree,
    Edit
  },
  methods:{
    setPageData() {
      getChildList(this.selectedCode)
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
        })
        .catch(err => {});
    },
    handleDelete(params) {
      var row = params.row;
      this.$Modal.confirm({
        title: "提示",
        content: "<p>确定要删除[" + row.functionCode + "]?</p>",
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
    remove(row) {
      var id = row.functionCode;
      remove(id)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("删除成功");
            this.reloadAll(this.selectedCode);
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => {});
    },
    saveEdit() {
      var row = this.$refs.edit.Row;
    },
    functionChange(code){
      this.selectedCode=code;
      this.setPageData();
    },
    reloadAll(parnetCode){
      this.$refs.functionTree.getFunctions();
      this.functionChange(parnetCode);
    }
  },
  mounted(){
      this.setPageData();
  }
};
</script>