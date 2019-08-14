<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <Row>
        <Col span="24">
          <FormItem label="父级"  id="item-parentName">
            <Input
              search
              enter-button
              v-model="Row.parentName"
              @on-search="functionSelect"
            />
            <!-- <Tree :data="functionTree"></Tree> -->
            <!-- <Input type="password" v-model="Row.functionCode" /> -->
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="中文名" prop="functionChina">
            <Input v-model="Row.functionChina" />
          </FormItem>
        </Col>
      </Row>

      <Row>
        <Col span="24">
          <FormItem label="英文名" prop="functionEnglish">
            <Input v-model="Row.functionEnglish" />
          </FormItem>
        </Col>
      </Row>

      <Row>
        <Col span="24">
          <FormItem label="是否菜单" prop="menuFlag">
            <RadioGroup v-model="Row.menuFlag">
              <Radio :label="1">
                <span>是</span>
              </Radio>
              <Radio :label="0">
                <span>否</span>
              </Radio>
            </RadioGroup>
          </FormItem>
        </Col>
      </Row>

      <Row>
        <Col span="24">
          <FormItem label="URL" prop="urlString">
            <Input v-model="Row.urlString" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="排序号" prop="sortidx">
            <Input v-model="Row.sortidx" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="图标样式" prop="menuIcon">
            <Input v-model="Row.menuIcon" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="备注" prop="functionDescrip">
            <Input
              v-model="Row.functionDescrip"
              type="textarea"
              :autosize="{minRows: 2,maxRows: 2}"
            />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEdit=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>

    <Modal
      title="选择父级"
      :mask-closable="false"
      v-model="modelfunction"
      width="300"
      scrollable
      footer-hide
      id="parent-modal"
    >
     <div style="height: 500px;overflow: auto;">
        <function-tree :parent="this"></function-tree>
     </div>

    </Modal>
  </div>
</template>
<script>
import { getFunctions, add, edit } from "@/api/pubFunction";
import FunctionTree from "./FunctionTree";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  components: {
    FunctionTree
  },
  data() {
    return {
      Row: {},
      Roles: [],
      functions: [],
      functionTree: [],
      functionTreeItems: [],
      modelfunction: false,
      rule: {
        functionChina: [
          {
            required: true,
            message: "中文名必填",
            trigger: "blur"
          }
        ],
        functionEnglish: [
          {
            required: true,
            message: "英文名必填",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    functionSelect() {
      this.modelfunction = true;
    },
    functionChange(code, title) {
      this.Row.parentCode = code;
      this.Row.parentName = title;
      var itemParent = document.getElementById("item-parentName");
      itemParent.classList.remove("ivu-form-item-error");
      this.modelfunction = false;
    },
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          add(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode!="0"?this.Row.parentCode:this.Row.functionCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
          edit(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEdit = false;
                this.parent.reloadAll(this.Row.parentCode!="0"?this.Row.parentCode:this.Row.functionCode);
              } else {
                this.$Message.error({
                  content: msg,
                  duration: 10,
                  closable: true
                });
              }
            })
            .catch(err => {});
        }
      });
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.Row.menuFlag = this.Row.menuFlag === false ? 0 : 1;
    }
  },
  mounted() {}
};
</script>

